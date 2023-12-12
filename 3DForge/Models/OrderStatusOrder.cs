using Backend3DForge.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class OrderStatusOrder
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int OrderId { get; set; }
		public Order Order { get; set; }
		[Required]
		public OrderStatusType OrderStatus { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
	}
}
