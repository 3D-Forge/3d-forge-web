using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class Cart
	{
		[Key]
		public int CartId { get; set; }
		[Required]
		public int CatalogModelId { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public int UserId { get; set; }
	}
}
