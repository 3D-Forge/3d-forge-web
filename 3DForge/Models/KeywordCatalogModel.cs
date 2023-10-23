using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class KeywordCatalogModel
	{
		[Key]
		public int KeywordCatalogModelId { get; set; }
		[Required]
		public int CatalogModelId { get; set; }
		[Required]
		public int KeywordId { get; set; }
	}
}
