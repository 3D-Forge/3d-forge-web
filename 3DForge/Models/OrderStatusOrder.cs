using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class OrderStatusOrder
	{
		[Key]
		public int OrderStatusOrderID { get; set; }
		[Required]
		public int OrderId { get; set; }
		[Required]
		public int OrderStatusId { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
	}
}
