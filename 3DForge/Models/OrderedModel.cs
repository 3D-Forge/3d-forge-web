using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Backend3DForge.Models
{
	public class OrderedModel
	{
		[Key]
		public int OrderedModelId { get; set; }
		[Required]
		public int OrderId { get; set; }
		[Required]
		public int CatalogModelId { get; set; }
		[Required]
		public int PrintExtensionId { get; set; }
		[Required]
		public float PricePerPiece { get; set; }
		[Required]
		public int Pieces {  get; set; }
		[Required]
		public float Height { get; set; }
		[Required]
		public float Width { get; set; }
		[Required]
		public float Depth { get; set; }
		[Required]
		public string Color { get; set; }
		[Required]
		public int PrintTypeId { get; set; }
		[Required]
		public int PrintMaterialId { get; set; } 
	}
}
