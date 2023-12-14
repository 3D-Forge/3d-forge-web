using Backend3DForge.Attributes;
using Backend3DForge.Enums;
using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Responses;
using Backend3DForge.Services.FileStorage;
using Backend3DForge.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Backend3DForge.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : BaseController
    {
        protected readonly IFileStorage fileStorage;

        public OrdersController(DbApp db, IFileStorage fileStorage) : base(db)
        {
            this.fileStorage = fileStorage;
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var orders = await DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderedModels).ThenInclude(p => p.PrintMaterialColor)
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
                .Include(p => p.PrintMaterialColor)
                .SingleOrDefaultAsync(p => p.Id == request.Id && p.Cart != null && p.Cart.UserId == AuthorizedUserId);

            if (orderModel == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected ordered model does not found."));
            }

            if (request.PrintType is not null)
            {
                var printTypeName = await DB.PrintTypes.FirstOrDefaultAsync(x => x.Id == request.PrintType);

                if (printTypeName is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("This print type does not exists"));
                }

                orderModel.PrintTypeId = printTypeName.Id;
            }

            if (request.PrintMaterial is not null)
            {
                var printMaterialName = await DB.PrintMaterials.FirstOrDefaultAsync(x => x.Id == request.PrintMaterial);

                if (printMaterialName is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse("This print material does not exists"));
                }

                orderModel.PrintMaterialId = printMaterialName.Id;
            }

            orderModel.Scale = request.Scale ?? orderModel.Scale;
            orderModel.Depth = request.Depth ?? orderModel.Depth;

            if (request.PrintColor is not null)
            {
                var color = await DB.PrintMaterialColors.SingleOrDefaultAsync(p => p.PrintMaterialId == orderModel.PrintMaterialId && p.Id == request.PrintColor);
                if (color is null)
                {
                    return BadRequest(new BaseResponse.ErrorResponse($"This color [{request.PrintColor}] does not exists in material [{orderModel.PrintMaterialId}]"));
                }
                orderModel.PrintMaterialColorId = color.Id;
            }

            await DB.SaveChangesAsync();

            return Ok(new OrderedModelResponse(orderModel));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> OrderModel([FromBody] OrderRequest request)
        {
            var cart = await DB.Carts
                .Include(p => p.User)
                .Include(p => p.OrderedModels)
                .FirstOrDefaultAsync(p => p.Id == request.CartId && p.UserId == AuthorizedUserId);

            if (cart is null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected cart does not found."));
            }

            if (cart.OrderedModels.Count == 0)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Cart is empty."));
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

            order = (await DB.Orders.AddAsync(order)).Entity;

            await DB.SaveChangesAsync();

            var orderStatusOrder = new OrderStatusOrder();
            orderStatusOrder.OrderId = order.Id;
            orderStatusOrder.OrderStatus = OrderStatusType.Paid;

            DB.OrderStatusOrders.Add(orderStatusOrder);

            await DB.SaveChangesAsync();

            foreach (var orderedModel in cart.OrderedModels)
            {
                orderedModel.OrderId = order.Id;
                orderedModel.CartId = null;
            }
            await DB.SaveChangesAsync();

            order = DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderedModels).ThenInclude(p => p.PrintMaterialColor)
                .Include(p => p.OrderStatusOrders)
                .FirstOrDefault(p => p.Id == order.Id);

            return Ok(new OrderResponse(order));
        }

        [Authorize]
        [CanRetrieveDelivery]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, [FromBody] UpdateOrderStatusRequest request)
        {
            if(Enum.GetName(typeof(OrderStatusType), request.Status) is null)
            {
                var values = Enum.GetValues<OrderStatusType>();
                var names = values.Select(p => $"{Enum.GetName(typeof(OrderStatusType), p)}[{(int)p}]");
                return BadRequest(new BaseResponse.ErrorResponse($"Invalid status. Supported only: {string.Join(", ", names)}"));
            }

            var order = await DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderStatusOrders)
                .Include(p => p.OrderedModels).ThenInclude(p => p.PrintMaterialColor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (order == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected order does not exists"));
            }

            var lastStatus = order.OrderStatusOrders.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
            if (lastStatus is not null && lastStatus.OrderStatus == request.Status)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Order already has this status"));
            }

            var orderStatusOrder = new OrderStatusOrder();
            orderStatusOrder.OrderId = order.Id;
            orderStatusOrder.CreatedAt = DateTime.UtcNow;
            orderStatusOrder.OrderStatus = request.Status;

            orderStatusOrder = (await DB.OrderStatusOrders.AddAsync(orderStatusOrder)).Entity;

            await DB.SaveChangesAsync();

            return Ok(new OrderResponse(order));
        }

        [Authorize]
        [CanRetrieveDelivery]
        [HttpGet("list")]
        public async Task<IActionResult> GetOrdersList([FromQuery] ListOrdersRequest request)
        {
            IQueryable<Order> orders = DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderStatusOrders)
                .Include(p => p.OrderedModels).ThenInclude(p => p.PrintMaterialColor);

            if(request.OrderStatus is not null)
            {
                orders = orders.Where(p => p.OrderStatusOrders.OrderByDescending(p => p.CreatedAt).First().OrderStatus == request.OrderStatus);
            }

            if(request.Query is not null)
            {
                orders = orders.Where(p => p.Firstname.Contains(request.Query) || p.Midname.Contains(request.Query) || p.Id.ToString().Contains(request.Query));
            }

            switch (request.SortParameter)
            {
                case "createdAt":
                    orders = request.SortDirection == "asc" ? orders.OrderBy(p => p.CreatedAt) : orders.OrderByDescending(p => p.CreatedAt);
                    break;
                case "status":
                    orders = request.SortDirection == "asc" ? 
                        orders.OrderBy(p => p.OrderStatusOrders.OrderByDescending(p => p.CreatedAt).First().OrderStatus) :
                        orders.OrderByDescending(p => p.OrderStatusOrders.OrderByDescending(p => p.CreatedAt).First().OrderStatus);
                    break;
                default:
                    orders = orders.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            orders = orders.ToPaged(request, out int count);

            var result = await orders.Select(p => new OrderResponse.View(p)).ToListAsync();

            return Ok(new PageResponse<OrderResponse.View>(result, request, count));
        }

        [Authorize]
        [CanRetrieveDelivery]
        [HttpGet("{orderId}/{modelId}/download-print-file")]
        public async Task<IActionResult> DownloadPrintFile([FromRoute] int orderId, [FromRoute] int modelId)
        {
            var order = await DB.Orders
                .Include(p => p.User)
                .Include(p => p.OrderedModels).ThenInclude(p => p.PrintMaterialColor)
                .Include(p => p.OrderedModels).ThenInclude(p => p.CatalogModel)
                .FirstOrDefaultAsync(p => p.Id == orderId && p.UserId != null);
            if (order == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected order does not exists"));
            }
            var orderedModel = order.OrderedModels.FirstOrDefault(p => p.Id == modelId);
            if (orderedModel == null)
            {
                return BadRequest(new BaseResponse.ErrorResponse("Selected model does not exists"));
            }
            var fileStream = orderedModel.CatalogModel is null ? 
                await fileStorage.DownloadPrintFile(orderedModel) : 
                await fileStorage.DownloadPrintFile(orderedModel.CatalogModel);
            HttpContext.Response.ContentLength = orderedModel.FileSize;
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=order-{order.Id}-{orderedModel.Id}.{orderedModel.PrintExtensionId}");
            return new FileStreamResult(fileStream, "application/octet-stream");
        }
    }
}
