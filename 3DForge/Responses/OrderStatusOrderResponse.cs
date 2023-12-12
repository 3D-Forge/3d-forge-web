using Backend3DForge.Models;
using System.Linq;

namespace Backend3DForge.Responses
{
	public class OrderStatusOrderResponse : BaseResponse
	{
		public OrderStatusOrderResponse(OrderStatusOrder orderStatusOrder) : base(true, null, null)
		{
			Data = new View(orderStatusOrder);
		}

		public OrderStatusOrderResponse(IEnumerable<OrderStatusOrder> orders) : base(true, null, null)
		{
			Data = orders.Select(p => new View(p));
		}

		public class View
		{
			public int Id { get; set; }
			public OrderResponse.View Order { get; set; }
			public string OrderStatusName { get; set; }
			public int OrderStatus { get; set; }

			public View(OrderStatusOrder orderStatusOrder)
			{
				Id = orderStatusOrder.Id;
				Order = new OrderResponse.View(orderStatusOrder.Order);
				OrderStatusName = Enum.GetName(orderStatusOrder.OrderStatus) ?? "unknown";
				OrderStatus = (int)orderStatusOrder.OrderStatus;
			}
		}
	}
}
