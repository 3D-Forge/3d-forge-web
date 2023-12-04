using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests;
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
                return NotFound("Selected order does not found.");
            }

            return Ok(new OrderResponse(order));
        }

		[Authorize]
        [HttpPost("change")]
        public async Task<IActionResult> ChangeProperties([FromBody] ChangeOrderedModelRequest request)
        {
            var orderModel = await DB.OrderedModels
                .Include(p => p.Cart)
                .Include(p => p.PrintType)
                .Include(p => p.PrintMaterial)
                .SingleOrDefaultAsync(p => p.Id == request.Id && p.Cart.UserId == AuthorizedUserId);

            if (orderModel == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected ordered model does not found."));
            }

            if (request.PrintType is not null)
            {
                var printTypeName = await DB.PrintTypes.FirstOrDefaultAsync(x => x.Name == request.PrintType);

                if (printTypeName is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("This print type does not exists"));
                }

                orderModel.PrintTypeName = printTypeName.Name;
            }

            if (request.PrintMaterial is not null)
            {
                var printMaterialName = await DB.PrintMaterials.FirstOrDefaultAsync(x => x.Name == request.PrintMaterial);

                if (printMaterialName is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("This print material does not exists"));
                }

                orderModel.PrintMaterialName = printMaterialName.Name;
            }

            orderModel.Scale = request.Scale ?? orderModel.Scale;
            orderModel.Depth = request.Depth ?? orderModel.Depth;
            orderModel.Color = request.PrintColor ?? orderModel.Color;

            await DB.SaveChangesAsync();

            return Ok(new OrderedModelResponse(orderModel));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> OrderModel([FromBody] OrderRequest request)
        {
            var cart = await DB.Carts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == request.CartId && p.UserId == AuthorizedUserId);

            if (cart is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected cart does not found."));
            }

            if (request.Country is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Country can't be null"));
            }
            else if (request.Region is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Region can't be null"));
            }
            else if (request.City is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("City can't be null"));
            }

            var order = new Order();
            order.CreatedAt = DateTime.UtcNow;
			order.UserId = AuthorizedUserId;
            order.Firstname = request.Firstname;
            order.Midname = request.Midname;
            order.Lastname = request.Lastname;
			order.Country = request.Country;
            order.Region = request.Region;
            order.City = request.City;

            if (request.DepartmentNumber is not null)
            {
                order.DeliveryType = "department";
				order.DepartmentNumber = request.DepartmentNumber;
			}
            else if (request.PostMachineNumber is not null)
            {
                order.DeliveryType = "postMachine";
                order.PostMachineNumber = request.PostMachineNumber;
			}
            else
            {
                order.DeliveryType = "courier";

                if (request.CityRegion is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("CityRegion can't be null"));
                }
                if (request.Street is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("Street can't be null"));
				}
				if (request.House is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("House can't be null"));
				}
				if (request.Apartment is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("Apartment can't be null"));
				}

                order.CityRegion = request.CityRegion;
                order.Street = request.Street;
                order.House = request.House;
                order.Apartment = request.Apartment;
			}

            var orderStatus = new OrderStatus("ordered");

			order = (await DB.Orders.AddAsync(order)).Entity;

			await DB.SaveChangesAsync();

			var orderStatusOrder = new OrderStatusOrder();
			orderStatusOrder.OrderId = order.Id;
            orderStatusOrder.OrderStatusName = orderStatus.Name;

            DB.OrderStatusOrders.Add(orderStatusOrder);

            await DB.SaveChangesAsync();

            // update cart`s ordered models
            var orderedModels = await DB.OrderedModels
                .Include(p => p.Cart)
                .Where(p => p.CartId == cart.Id && p.OrderId != null)
                .ToArrayAsync();

            foreach (var orderedModel in orderedModels)
            {
                orderedModel.OrderId = order.Id;
            }
            await DB.SaveChangesAsync();

            return Ok(new OrderStatusOrderResponse(orderStatusOrder));
        }

        [Authorize]
        [CanRetrieveDelivery]
		[HttpPost("orderStatus")]
		public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            var orderStatusOrder = await DB.OrderStatusOrders
                .Include(p => p.OrderStatus)
                .Include(p => p.Order)
                .Include(p => p.Order.User)
				.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (orderStatusOrder == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected order status order does not exists"));
            }

            var orderStatus = await DB.OrderStatuses
                .FirstOrDefaultAsync(x => x.Name == request.Status);

            if (orderStatus == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected order status does not exists"));
            }

            orderStatusOrder.OrderStatusName = orderStatus.Name;

            return Ok(new OrderStatusOrderResponse(orderStatusOrder));
		}
    }
}
