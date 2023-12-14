using Backend3DForge.Attributes;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.FileStorage;
using Backend3DForge.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserResponse = Backend3DForge.Responses.Administrate.UserResponse;

namespace Backend3DForge.Controllers
{
    [CanAdministrateSystem]
    [Route("api/administration/users")]
    [ApiController]
    public class AdministrateUsersController : BaseController
    {
        protected readonly IFileStorage fileStorage;
        public AdministrateUsersController(DbApp db, IFileStorage fileStorage) : base(db)
        {
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageRequest request, [FromQuery(Name = "query")] string? queryText = null)
        {
            var query = DB.Users.AsQueryable();
            if (!string.IsNullOrEmpty(queryText))
                query = query.Where(x => x.Email.Contains(queryText) || x.Login.Contains(queryText));

            var result = await query
                .ToPaged(request, out var totalItemsCount)
                .Select(p => new UserResponse.View(p))
                .ToListAsync();

            return Ok(new PageResponse<UserResponse.View>(result, request.Page, request.PageSize, (int)Math.Ceiling((double)totalItemsCount / request.PageSize)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await DB.Users.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (result == null)
                return NotFound(new BaseResponse.ErrorResponse($"{id} not found"));
            return Ok(new UserResponse(result));
        }

        [HttpPut("{id}/blocked")]
        public async Task<IActionResult> Blocked([FromRoute] int id)
        {
            var result = await DB.Users.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (result == null)
                return NotFound(new BaseResponse.ErrorResponse($"{id} not found"));
            if (result.Id.Equals(1))
                return BadRequest(new BaseResponse.ErrorResponse($"Can't block admin user"));
            if (result.Id.Equals(AuthorizedUserId))
                return BadRequest(new BaseResponse.ErrorResponse($"Can't block yourself"));
            result.Blocked = !result.Blocked;
            await DB.SaveChangesAsync();
            return Ok(new UserResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await DB.Users.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (result == null)
                return NotFound(new BaseResponse.ErrorResponse($"{id} not found"));
            if (result.Id.Equals(1))
                return BadRequest(new BaseResponse.ErrorResponse($"Can't delete admin user"));
            if (result.Id.Equals(AuthorizedUserId))
                return BadRequest(new BaseResponse.ErrorResponse($"Can't delete yourself"));

            // clean cart
            var cart = DB.Carts.SingleOrDefault(p => p.UserId == id);
            if (cart != null)
            {
                var cartItems = DB.OrderedModels.Where(p => p.CartId == cart.Id);
                DB.OrderedModels.RemoveRange(cartItems);
                DB.Carts.Remove(cart);
                await DB.SaveChangesAsync();
            }

            // clean orders
            var orders = DB.Orders.Where(p => p.UserId == id);
            foreach (var order in orders)
            {
                order.UserId = null;
                order.User = null;
            }
            await DB.SaveChangesAsync();

            var modelsFromCatalog = DB.CatalogModels.Where(p => p.UserId == id);
            foreach (var model in modelsFromCatalog)
            {
                model.UserId = null;
                model.User = null;
                model.ModelFileSize = 0;
                model.PrintFileSize = 0;
                await DB.SaveChangesAsync();

                await fileStorage.DeletePreviewModel(model);
                await fileStorage.DeletePrintFile(model);
            }

            // comment - writer as deleted user
            var comments = DB.CatalogModelFeedbacks.Where(p => p.UserId == id);
            foreach (var comment in comments)
            {
                comment.UserId = null;
                comment.User = null;
                await DB.SaveChangesAsync();
            }

            // thread - writer as deleted user
            var threads = DB.ForumThreads.Where(p => p.UserId == id);
            foreach (var thread in threads)
            {
                thread.UserId = null;
                thread.User = null;
                await DB.SaveChangesAsync();
            }
          
            // posts - writer as deleted user
            var posts = DB.Posts.Where(p => p.UserId == id);
            foreach (var post in posts)
            {
                post.UserId = null;
                post.User = null;
                await DB.SaveChangesAsync();
            }

            DB.Users.Remove(result);
            await DB.SaveChangesAsync();
            return Ok(new UserResponse(result));
        }
    }
}
