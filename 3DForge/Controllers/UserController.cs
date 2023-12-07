using Backend3DForge.Attributes;
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
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
                    templateName: "confirm_email_after_registration.html",
                    parameters: new Dictionary<string, string>
                    {
                        { "login", request.Login },
                        { "link", $"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(request.Email)}?token={token}" }
                    });
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
                OrderStateChangedNotification = true,
                GetForumResponseNotification = true,
                ModelRatedNotification = true,
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

        [Authorize]
        [HttpPut("change-email")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
        {
            User user = AuthorizedUser;

            if (!PasswordTool.Validate(request.Password, user.PasswordHash))
            {
                return BadRequest(new BaseResponse.ErrorResponse("Invalid password!"));
            }

            string token = StringTool.RandomString(256);

            try
            {
                await emailService.SendEmailUseTemplateAsync(
                    email: request.Email,
                    templateName: "confirm_email.html",
                    parameters: new Dictionary<string, string>
                    {
                        { "login", user.Login },
                        { "link", $"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(user.Email)}?token={token}" }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
            }

            DateTime time = DateTime.Now;

            await DB.ActivationCodes.AddAsync(new ActivationCode
            {
                UserId = user.Id,
                User = user,
                Code = token,
                Action = $"change-email,{request.Email}",
                CreatedAt = time,
                Expires = time.AddHours(12)
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
            return Redirect("/");
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Ok(new UserRightResponse(true, null, AuthorizedUser));
            }
            return Unauthorized(new BaseResponse.ErrorResponse(null));
        }

        [HttpPost("{userLogin}/send-reset-password-permission")]
        public async Task<IActionResult> SendResetPasswordPermission(string userLogin)
        {
            User? user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);

            if (user == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("A user is not found!"));
            }

            string token = StringTool.RandomString(256);

            try
            {
                await emailService.SendEmailUseTemplateAsync(
                    email: user.Email,
                    templateName: "reset_password_permission.html",
                    parameters: new Dictionary<string, string>
                    {
                        { "login", user.Login },
                        { "link", $"https://{HttpContext.Request.Host}/reset-password?login={user.Login}&token={token}" }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
            }

            DateTime time = DateTime.Now;

            await DB.ActivationCodes.AddAsync(new ActivationCode
            {
                UserId = user.Id,
                User = user,
                Code = token,
                Action = "reset-password-permission",
                CreatedAt = time,
                Expires = time.AddHours(12)
            });

            await DB.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse("Email is sent!"));
        }

        [HttpPut("{userLogin}/reset-password")]
        public async Task<IActionResult> ResetPassword(string userLogin, [FromBody] ResetPasswordRequest request)
        {
            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Too short password!"));
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest(new BaseResponse.ErrorResponse("The password is not confirmed!"));
            }

            User? user;

            if (!request.Token.IsNullOrEmpty())
            {
                ActivationCode? activationCode = await DB.ActivationCodes
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.User.Login == userLogin && p.Code == request.Token);

                if (activationCode == null)
                {
                    return NotFound(new BaseResponse.ErrorResponse("The link is expired or unavailable!"));
                }

                if (DateTime.Now > activationCode.Expires)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("The link is expired!"));
                }

                user = activationCode.User;
                user.PasswordHash = PasswordTool.Hash(request.NewPassword);

                DB.ActivationCodes.Remove(activationCode);
            }
            else
            {
                user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);

                if (user == null)
                {
                    return NotFound(new BaseResponse.ErrorResponse("A user is not found!"));
                }

                if (!PasswordTool.Validate(request.OldPassword ?? "", user.PasswordHash))
                {
                    return BadRequest(new BaseResponse.ErrorResponse("Invalid password!"));
                }

                user.PasswordHash = PasswordTool.Hash(request.NewPassword);
            }

            await DB.SaveChangesAsync();
            return Ok(new BaseResponse.SuccessResponse("The password is changed!"));
        }

        [Authorize]
        [HttpGet("self/info")]
        public IActionResult GetSelfInfo()
        {
            return Ok(new UserResponse(true, null, AuthorizedUser));
        }

        [CanAdministrateSystem]
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

        [Authorize]
        [HttpGet("self/avatar")]
        public async Task<IActionResult> GetSelfAvatar()
        {
            Stream fileStream;

            try
            {
                fileStream = await fileStorage.DownloadAvatarAsync(AuthorizedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message, ex));
            }

            return new FileStreamResult(fileStream, "image/png");
        }

        [CanAdministrateSystem]
        [HttpGet("{userLogin}/avatar")]
        public async Task<IActionResult> GetUserAvatar(string userLogin)
        {
            User? user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);

            if (user == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("A user is not found!"));
            }

            Stream fileStream;

            try
            {
                fileStream = await fileStorage.DownloadAvatarAsync(AuthorizedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message, ex));
            }

            return new FileStreamResult(fileStream, "image/png");
        }

        [Authorize]
        [CanAdministrateSystem]
        [HttpPut("update/info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest request, [FromQuery(Name = "login")] string? userLogin = null)
        {
            var user = AuthorizedUser;

            if (userLogin is not null)
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
            if (user.PhoneNumber != (request.PhoneNumber ?? user.PhoneNumber))
            {
                if (!string.IsNullOrEmpty(request.PhoneNumber) && !Regex.IsMatch(request.PhoneNumber, "^[\\+]?[0-9]{3}[0-9]{3}[0-9]{4,6}$"))
                {
                    return BadRequest(new BaseResponse.ErrorResponse("Invalid phone number"));
                }
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
            if (user.OrderStateChangedNotification != request.OrderStateChangedNotification)
            {
                user.OrderStateChangedNotification = request.OrderStateChangedNotification;
                different = true;
            }
            if (user.GetForumResponseNotification != request.GetForumResponseNotification)
            {
                user.GetForumResponseNotification = request.GetForumResponseNotification;
                different = true;
            }
            if (user.ModelRatedNotification != request.ModelRatedNotification)
            {
                user.ModelRatedNotification = request.ModelRatedNotification;
                different = true;
            }

            if (!different)
            {
                return Ok(new BaseResponse.SuccessResponse("Data are identical"));
            }

            await DB.SaveChangesAsync();

            return Ok(new UserResponse(true, "Data Updated", user));
        }

        [Authorize]
        [CanAdministrateSystem]
        [HttpPut("update/avatar")]
        public async Task<IActionResult> UpdateUserAvatar(IFormFile userAvatarFile, [FromQuery(Name = "login")] string? userLogin = null)
        {
            var user = AuthorizedUser;

            if (userLogin is not null)
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

            if (userAvatarFile.Length > 1024 * 1024 * 10)
            {
                return BadRequest(new BaseResponse.ErrorResponse($"File is too large. Max size: 10MB"));
            }

            await fileStorage.DeleteAvatarAsync(user);
            await fileStorage.UploadAvatarAsync(user, userAvatarFile.OpenReadStream(), userAvatarFile.Length);

            return Ok(new UserResponse(true, "Avatar uploaded", user));
        }
    }
}
