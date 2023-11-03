using Backend3DForge.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend3DForge.Controllers
{
    public class BaseController : ControllerBase
    {
        protected DbApp DB { get; }
        protected User AuthorizedUser
        {
            get
            {
                var strId = HttpContext.User.Claims
                    .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(strId, out int id))
                {
                    throw new InvalidOperationException("This property accessible only for authorized users.");
                }

                User? user = DB.Users.SingleOrDefault(p => p.Id == id);

                if(user is null)
                {
                    throw new InvalidOperationException("This property accessible only for authorized users.");
                }

                return user;
            }
        }

        public BaseController(DbApp db)
        {
            DB = db;
        }
    }
}
