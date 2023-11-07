using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class PrintExtension
	{
		[Key]
		public string Name { get; set; }

		public PrintExtension() { }

		public PrintExtension(string name)
		{
			Name = name;
		}

		public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();
		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
	}
}
