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
		public double PricePerPiece { get; set; }
		[Required]
		public int Pieces { get; set; }
		[Required]
		public float XSize { get; set; }
		[Required]
		public float YSize { get; set; }
		[Required]
		public float ZSize { get; set; }
		[Required]
		public double Volume { get; set; } = 0;
		[Required]
		public double Scale { get; set; } = 1;
		[Required]
		public float Depth { get; set; }
		[Required]
		public int PrintMaterialColorId{ get; set; }
		public PrintMaterialColor PrintMaterialColor { get; set; }
		[Required]
		public string PrintTypeName { get; set; }
		public PrintType PrintType { get; set; }
		[Required]
		public string PrintMaterialName { get; set; }
		public PrintMaterial PrintMaterial { get; set; }
		public int? CartId { get; set; }
		public Cart? Cart { get; set; }

		public ICollection<CatalogModelFeedback> CatalogModelResponses { get; set; } = new List<CatalogModelFeedback>();
	}
}
