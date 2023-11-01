using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.Email;
using Backend3DForge.Services.FileStorage;
using Backend3DForge.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Backend3DForge.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IEmailService emailService;
        private readonly IFileStorage fileStorage;

        public UserController(DbApp db, IEmailService emailService, IFileStorage fileStorage) : base(db)
        {
            this.emailService = emailService;
            this.fileStorage = fileStorage;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await DB.Users.AnyAsync(p => p.Login == request.Login))
            {
                return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same login!"));
            }

            if (await DB.Users.AnyAsync(p => p.Email == request.Email))
            {
                return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same email!"));
            }

            if (request.Password.Length < 6)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Too short password!"));
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new BaseResponse.ErrorResponse("The password is not confirmed!"));
            }

            string token = StringTool.RandomString(256);

            try
            {
                await emailService.SendEmailUseTemplateAsync(
                    email: request.Email,
                    tepmlateName: "confirm_email_after_registration.html",
                    parameters: new Dictionary<string, string>
                    {
                        { "login", request.Login },
                        { "link", $"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(request.Email)}?token={token}" }
                    }
                    );
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
            }

            User user = (await DB.Users.AddAsync(new User
            {
                Login = request.Login,
                PasswordHash = PasswordTool.Hash(request.Password),
                Email = request.Email,
                Blocked = false,
                CanAdministrateForum = false,
                CanModerateCatalog = false,
                CanRetrieveDelivery = false,
                CanAdministrateSystem = false,
                IsActivated = false,
                RegistrationDate = DateTime.Now
            })).Entity;

            await DB.ActivationCodes.AddAsync(new ActivationCode
            {
                UserId = user.Id,
                User = user,
                Code = token,
                Action = "confirm-registration",
                CreatedAt = user.RegistrationDate,
                Expires = user.RegistrationDate.AddHours(12)
            });

            await DB.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse("Email is sent!"));
        }

        [HttpGet("confirm-email/{email}")]
        public async Task<IActionResult> ConfirmEmail(string email, [FromQuery] string? token)
        {
            ActivationCode? activationCode = await DB.ActivationCodes
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.User.Email == WebUtility.UrlDecode(email) && p.Code == token);

            if (activationCode == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("The link is expired or unavailable!"));
            }

            if (DateTime.Now > activationCode.Expires)
            {
                return BadRequest(new BaseResponse.ErrorResponse("The link is expired!"));
            }

            User user = activationCode.User;
            var splited = activationCode.Action.Split(',');

            switch (splited[0])
            {
                case "confirm-registration":
                    user.IsActivated = true;
                    break;
                case "change-email":
                    user.Email = splited[1];
                    break;
                default:
                    return BadRequest(new BaseResponse.ErrorResponse("Unknown action"));
            }

            DB.ActivationCodes.Remove(activationCode);
            await DB.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            User? user = await DB.Users.FirstOrDefaultAsync(
                p => p.Login == request.LoginOrEmail || p.Email == request.LoginOrEmail);

            if (user == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("Invalid login or email!"));
            }

            if (!PasswordTool.Validate(request.Password, user.PasswordHash))
            {
                return BadRequest(new BaseResponse.ErrorResponse("Invalid password!"));
            }

            if (user.Blocked)
            {
                return Unauthorized(new BaseResponse.ErrorResponse("The user is blocked!"));
            }

            if (!user.IsActivated)
            {
                return Unauthorized(new BaseResponse.ErrorResponse("The user is not activated!"));
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim("can_administrate_forum", user.CanAdministrateForum.ToString()),
                new Claim("can_retrieve_delivery", user.CanRetrieveDelivery.ToString()),
                new Claim("can_moderate_catalog", user.CanModerateCatalog.ToString()),
                new Claim("can_administrate_system", user.CanAdministrateSystem.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme)));

            return Ok(new UserResponse(true, null, user));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Ok(new BaseResponse.SuccessResponse("Authorized", null));
            }
            return Unauthorized(new BaseResponse.ErrorResponse("Unauthorized"));
        }

        [HttpGet("self/info")]
        public IActionResult GetSelfInfo()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Ok(new UserResponse(true, null, AuthorizedUser));
            }

            return Unauthorized(new BaseResponse.ErrorResponse("The user is not authorized!"));
        }

        [HttpGet("{userLogin}/info")]
        public async Task<IActionResult> GetUserInfo(string userLogin)
        {
            User? user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);

            if (user == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("A user is not found!"));
            }

            return Ok(new UserResponse(true, null, user));
        }

        [HttpGet("self/avatar")]
        public async Task<IActionResult> GetSelfAvatar()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                Stream fileStream = await fileStorage.DownloadAvatarAsync(AuthorizedUser);
                return new FileStreamResult(fileStream, "image/png");
            }

            return Unauthorized(new BaseResponse.ErrorResponse("The user is not authorized!"));
        }

        [HttpGet("{userLogin}/avatar")]
        public async Task<IActionResult> GetUserAvatar(string userLogin)
        {
            User? user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);

            if (user == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("A user is not found!"));
            }

            Stream fileStream = await fileStorage.DownloadAvatarAsync(user);
            return new FileStreamResult(fileStream, "image/png");
        }

        [Authorize]
        [HttpPut("update/info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest request, [FromQuery(Name = "login")] string? userLogin = null)
        {
            var user = AuthorizedUser;

            if (userLogin is not null && AuthorizedUser.CanAdministrateSystem)
            {
                user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);
                if (user == null)
                {
                    return NotFound(new BaseResponse.ErrorResponse("User login not found"));
                }
            }

            if (user == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("User login not found"));
            }

            bool different = false;

            if (user.Login != (request.Login ?? user.Login))
            {
                if (await DB.Users.AnyAsync(p => p.Login == request.Login))
                {
                    return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same login!"));
                }

                user.Login = request.Login;
                different = true;
            }
            if (user.Email != (request.Email ?? user.Email))
            {
                if (await DB.Users.AnyAsync(p => p.Email == request.Email))
                {
                    return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same email!"));
                }

                string token = StringTool.RandomString(256);

                try
                {
                    await emailService.SendEmailUseTemplateAsync(
                        email: request.Email,
                        tepmlateName: "confirm_email.html",
                        parameters: new Dictionary<string, string>
                        {
                            { "login", user.Login },
                            { "link", $"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(request.Email)}?token={token}" },
                        }
                        );
                }
                catch (Exception ex)
                {
                    return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
                }

                await DB.ActivationCodes.AddAsync(new ActivationCode
                {
                    UserId = user.Id,
                    User = user,
                    Code = token,
                    Action = "change-email," + request.Email,
                    CreatedAt = DateTime.Now,
                    Expires = DateTime.Now.AddHours(12)
                });

                different = true;
            }
            if (user.PhoneNumber != (request.PhoneNumber ?? user.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
                different = true;
            }
            if (user.Firstname != (request.Firstname ?? user.Firstname))
            {
                user.Firstname = request.Firstname;
                different = true;
            }
            if (user.Midname != (request.Midname ?? user.Midname))
            {
                user.Midname = request.Midname;
                different = true;
            }
            if (user.Lastname != (request.Lastname ?? user.Lastname))
            {
                user.Lastname = request.Lastname;
                different = true;
            }
            if (user.Region != (request.Region ?? user.Region))
            {
                user.Region = request.Region;
                different = true;
            }
            if (user.CityRegion != (request.CityRegion ?? user.CityRegion))
            {
                user.CityRegion = request.CityRegion;
                different = true;
            }
            if (user.City != (request.City ?? user.City))
            {
                user.City = request.City;
                different = true;
            }
            if (user.Street != (request.Street ?? user.Street))
            {
                user.Street = request.Street;
                different = true;
            }
            if (user.House != (request.House ?? user.House))
            {
                user.House = request.House;
                different = true;
            }
            if (user.Apartment != (request.Apartment ?? user.Apartment))
            {
                user.Apartment = request.Apartment;
                different = true;
            }
            if (user.DepartmentNumber != (request.DepartmentNumber ?? user.DepartmentNumber))
            {
                user.DepartmentNumber = request.DepartmentNumber;
                different = true;
            }
            if (user.DeliveryType != (request.DeliveryType ?? user.DeliveryType))
            {
                user.DeliveryType = request.DeliveryType;
                different = true;
            }

            if (!different)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Data are identical"));
            }

            await DB.SaveChangesAsync();

            return Ok(new UserResponse(true, "Data Updated", user));
        }

        [Authorize]
        [HttpPut("update/avatar")]
        public async Task<IActionResult> UpdateUserAvatar(IFormFile userAvatarFile, [FromQuery(Name = "login")] string? userLogin = null)
        {
            var user = AuthorizedUser;

            if (userLogin is not null && AuthorizedUser.CanAdministrateSystem)
            {
                user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);
                if (user == null)
                {
                    return NotFound(new BaseResponse.ErrorResponse("User login not found"));
                }
            }

            if (userAvatarFile.ContentType != "image/png")
            {
                return BadRequest(new BaseResponse.ErrorResponse("Accepted only .png images"));
            }

            await fileStorage.DeleteAvatarAsync(user);
            await fileStorage.UploadAvatarAsync(user, userAvatarFile.OpenReadStream(), userAvatarFile.Length);

            return Ok(new UserResponse(true, "Avatar uploaded", user));
        }
    }
}
