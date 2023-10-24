using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend3DForge.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        public UserController(DbApp db) : base(db)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            User? user = await DB.Users.FirstOrDefaultAsync(
                p => p.Login == request.LoginOrEmail || p.Email == request.LoginOrEmail);

            if (user == null)
            {
                return Unauthorized(new BaseResponse.ErrorResponse("Invalid login or email!", null));
            }

            if (!PasswordTool.Validate(request.Password, user.PasswordHash))
            {
                return Unauthorized(new BaseResponse.ErrorResponse("Invalid password!", null));
            }

            if (user.Blocked)
            {
                return Unauthorized(new BaseResponse.ErrorResponse("User is blocked!", null));
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
