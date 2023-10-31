using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.Email;
using Backend3DForge.Services.FileStorage;
using Backend3DForge.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        private void GenerateEmailActivation()
        {

        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (DB.Users.Any(p => p.Login == request.Login))
            {
                return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same login!"));
            }

            if (DB.Users.Any(p => p.Email == request.Email))
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
                await emailService.SendEmailAsync(
                    request.Email,
                    "Confirm registration",
                    "Hello,! You need to confirm your email to create " +
                    "an account by clicking the link below (the link will be expired in 12 hours).\n" +
                    $"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(request.Email)}?token={token}"
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

        [HttpPut("update/info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest request)
        {
            var result = DB.Users.SingleOrDefault(x => x.Id == request.Id);

            if (result == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("User ID not found"));
            }

			bool different = false;

            if (result.Login != (request.Login ?? result.Login))
            {
				if (DB.Users.Any(p => p.Login == request.Login))
				{
					return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same email!"));
				}

				result.Login = request.Login;
				different = true;
			}
            if (result.Email != (request.Email ?? result.Email))
            {
				if (DB.Users.Any(p => p.Email == request.Email))
				{
					return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same email!"));
				}

				string token = StringTool.RandomString(256);

				try
				{
					await emailService.SendEmailAsync(
						request.Email,
						"Confirm email change",
						"Hello,! You need to confirm your new email " +
						"by clicking the link below (the link will be expired in 12 hours).\n" +
						$"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(request.Email)}?token={token}",
						false);
				}
				catch (Exception ex)
				{
					return BadRequest(new BaseResponse.ErrorResponse(ex.Message));
				}

				await DB.ActivationCodes.AddAsync(new ActivationCode
				{
					UserId = result.Id,
					User = result,
					Code = token,
					Action = "change-email," + request.Email,
					CreatedAt = DateTime.Now,
					Expires = DateTime.Now.AddHours(12)
				});

				different = true;
			}
            if (result.PhoneNumber != (request.PhoneNumber ?? result.PhoneNumber))
            {
                result.PhoneNumber = request.PhoneNumber;
				different = true;
			}
			if (result.Firstname != (request.Firstname ?? result.Firstname))
            {
                result.Firstname = request.Firstname;
				different = true;
			}
			if (result.Midname != (request.Midname ?? result.Midname))
            {
                result.Midname = request.Midname;
				different = true;
			}
			if (result.Lastname != (request.Lastname ?? result.Lastname))
            {
                result.Lastname = request.Lastname;
				different = true;
			}
			if (result.Region != (request.Region ?? result.Region))
            {
                result.Region = request.Region;
				different = true;
			}
			if (result.CityRegion != (request.CityRegion ?? result.CityRegion))
            {
                result.CityRegion = request.CityRegion;
				different = true;
			}
			if (result.City != (request.City ?? result.City))
            {
                result.City = request.City;
				different = true;
			}
			if (result.Street != (request.Street ?? result.Street))
            {
                result.Street = request.Street;
				different = true;
			}
			if (result.House != (request.House ?? result.House))
            {
                result.House = request.House;
				different = true;
			}
			if (result.Apartment != (request.Apartment ?? result.Apartment))
            {
                result.Apartment = request.Apartment;
				different = true;
			}
			if (result.DepartmentNumber != (request.DepartmentNumber ?? result.DepartmentNumber))
            {
                result.DepartmentNumber = request.DepartmentNumber;
				different = true;
			}
			if (result.DeliveryType != (request.DeliveryType ?? result.DeliveryType))
            {
                result.DeliveryType = request.DeliveryType;
				different = true;
			}

			if (!different)
			{
				return BadRequest(new BaseResponse.ErrorResponse("Data are identical"));
			}

			DB.SaveChanges();

			return Ok(new UserResponse(true, "Data Updated", result));
        }

        [HttpPut("update/avatar")]
        public async Task<IActionResult> UpdateUserAvatar(string userLogin, IFormFile userAvatarFile)
        {
			User? user = await DB.Users.FirstOrDefaultAsync(p => p.Login == userLogin);

			if (user == null)
			{
				return NotFound(new BaseResponse.ErrorResponse("A user is not found!"));
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
