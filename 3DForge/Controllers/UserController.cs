using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.Email;
using Backend3DForge.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Backend3DForge.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IEmailService emailService;

        public UserController(DbApp db, IEmailService emailService) : base(db)
        {
            this.emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request.Password.Length < 6)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Too short password!", null));
            }

            if (DB.Users.Any(p => p.Login == request.Login))
            {
                return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same login!", null));
            }

            if (DB.Users.Any(p => p.Email == request.Email))
            {
                return BadRequest(new BaseResponse.ErrorResponse("There is a user with the same email!", null));
            }

            string token = StringTool.RandomString(256);

            try
            {
                await emailService.SendEmailAsync(
                    request.Email,
                    "Confirm registration",
                    $"Hello, {request.Login}! You need to confirm your email to create " +
                    $"an account by clicking the link below (the link will be expired in 12 hours).\n" +
                    $"https://{HttpContext.Request.Host}/api/user/confirm-email/{WebUtility.UrlEncode(request.Email)}?token={token}",
                    false);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse.ErrorResponse(ex.Message, null));
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
                Action = "confirm-email",
                CreatedAt = user.RegistrationDate,
                Expires = user.RegistrationDate.AddHours(12)
            });

            await DB.SaveChangesAsync();

            return Ok(new BaseResponse.SuccessResponse("Email is sent!", null));
        }

        [HttpGet("confirm-email/{email}")]
        public async Task<IActionResult> ConfirmEmail(string email, [FromQuery] string? token)
        {
            ActivationCode? activationCode = await DB.ActivationCodes
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.User.Email == WebUtility.UrlDecode(email) && p.Code == token);
            
            if (activationCode == null)
            {
                return NotFound(new BaseResponse.ErrorResponse("The link is expired or unavailable!", null));
            }

            if (DateTime.Now > activationCode.Expires)
            {
                return BadRequest(new BaseResponse.ErrorResponse("The link is expired!", null));
            }

            User user = activationCode.User;
            user.IsActivated = true;
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
                return NotFound(new BaseResponse.ErrorResponse("Invalid login or email!", null));
            }

            if (!PasswordTool.Validate(request.Password, user.PasswordHash))
            {
                return BadRequest(new BaseResponse.ErrorResponse("Invalid password!", null));
            }

            if (user.Blocked)
            {
                return Unauthorized(new BaseResponse.ErrorResponse("The user is blocked!", null));
            }

            if (!user.IsActivated)
            {
                return Unauthorized(new BaseResponse.ErrorResponse("The user is not activated!", null));
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
            return Unauthorized(new BaseResponse.ErrorResponse("Unauthorized", null));
        }
    }
}
