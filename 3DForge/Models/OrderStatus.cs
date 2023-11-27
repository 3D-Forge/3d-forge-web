using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class OrderStatus
	{
		[Key]
		public string Name { get; set; }

		public ICollection<OrderStatusOrder> OrderStatusOrders { get; set; } = new List<OrderStatusOrder>();
	}
}
