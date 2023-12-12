using Backend3DForge.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class UpdateOrderStatusRequest
	{
		[Required]
		public OrderStatusType Status { get; set; }
	}
}
