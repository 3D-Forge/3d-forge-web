using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class CatalogModelResponse
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int CatalogModelId { get; set; }
		public CatalogModel CatalogModel { get; set; }
		[Required]
		public int OrderId { get; set; }
		public OrderedModel Order { get; set; }
		[Required]
		public int Rate { get; set; }
		[Required]
		[MaxLength(512)]
		public string Text { get; set; }
		[MaxLength(512)]
		public string Pros { get; set; }
		[MaxLength(512)]
		public string Cons { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
	}
}
