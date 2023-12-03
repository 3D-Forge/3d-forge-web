using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class UpdateOrderStatusRequest
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string Status { get; set; }
	}
}
