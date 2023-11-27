using Backend3DForge.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : BaseController
    {
        public OrdersController(DbApp db) : base(db)
        {
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var orders = await DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderedModels)
                .Include(p => p.OrderStatusOrders)
                .Where(p => p.UserId == AuthorizedUserId)
                .ToArrayAsync();

            return Ok(new OrderResponse(orders));
        }

        [Authorize]
        [HttpGet("history/{id}")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var order = await DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderedModels)
                .Include(p => p.OrderStatusOrders)
                .SingleOrDefaultAsync(p => p.Id == id && p.UserId == AuthorizedUserId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(new OrderResponse(order));
        }
    }
}
