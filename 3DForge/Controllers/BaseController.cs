using Backend3DForge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend3DForge.Controllers
{
    public class BaseController : ControllerBase
    {
        protected DbApp DB { get; }
        protected User? AuthorizedUser
        {
            get
            {
                var strId = HttpContext.User.Claims
                    .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(strId, out int id))
                {
                    return null;
                }

                User? user = DB.Users.SingleOrDefault(p => p.Id == id);

                return user;
            }
        }

        public BaseController(DbApp db)
        {
            DB = db;
        }
    }
}
