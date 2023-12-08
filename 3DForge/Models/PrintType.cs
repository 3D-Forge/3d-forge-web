using Backend3DForge.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class PrintType : ITableKey<string>
	{
		[Key]
		public string Id { get; set; }

		public PrintType() { }

		public PrintType(string name)
		{
			Id = name;
		}

		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
		public ICollection<PrintMaterial> PrintMaterials { get; set; } = new List<PrintMaterial>();
	}
}
