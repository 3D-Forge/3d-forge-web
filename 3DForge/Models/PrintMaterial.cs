using Backend3DForge.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class PrintMaterial : ITableKey<string>
	{
		[Key]
		public string Id { get; set; }
		[Required]
		public string PrintTypeId { get; set; }
		public PrintType PrintType { get; set; }
		[Required]
		public float Density { get; set; }
		[Required]
		public float Cost { get; set; }

		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
		public ICollection<PrintMaterialColor> PrintMaterialColors { get; set; } = new List<PrintMaterialColor>();
	}
}
