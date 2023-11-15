using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class PrintType
	{
		[Key]
		public string Name { get; set; }

		public PrintType() { }

		public PrintType(string name)
		{
			Name = name;
		}

		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
		public ICollection<PrintMaterial> PrintMaterials { get; set; } = new List<PrintMaterial>();
	}
}
