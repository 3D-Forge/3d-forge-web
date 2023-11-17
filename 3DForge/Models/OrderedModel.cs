using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class OrderedModel
	{
		[Key]
		public int Id { get; set; }
		public int? OrderId { get; set; }
		public Order? Order { get; set; }
		public int? CatalogModelId { get; set; }
		public CatalogModel? CatalogModel { get; set; }
		[Required]
		public string PrintExtensionName { get; set; }
		public PrintExtension PrintExtension { get; set; }
		[Required]
		public float PricePerPiece { get; set; }
		[Required]
		public int Pieces { get; set; }
		[Required]
		public float Height { get; set; }
		[Required]
		public float Width { get; set; }
		[Required]
		public float Depth { get; set; }
		[Required]
		public string Color { get; set; }
		[Required]
		public string PrintTypeName { get; set; }
		public PrintType PrintType { get; set; }
		[Required]
		public string PrintMaterialName { get; set; }
		public PrintMaterial PrintMaterial { get; set; }
		public ICollection<CatalogModelFeedback> CatalogModelResponses { get; set; } = new List<CatalogModelFeedback>();
		public ICollection<Cart> Carts { get; set; } = new List<Cart>();
	}
}
