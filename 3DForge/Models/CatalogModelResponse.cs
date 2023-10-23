using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class CatalogModelResponse
	{
		[Key]
		public int CatalogModelResponseId { get; set; }
		[Required]
		public int CatalogModelId { get; set; }
		[Required]
		public int OrderId { get; set; }
		[Required]
		public int Rate { get; set; }
		[Required]
		public string ResponseText { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
	}
}
