using Backend3DForge.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class PrintExtension : ITableKey<string>
	{
		[Key]
		public string Id { get; set; }

		public PrintExtension() { }

		public PrintExtension(string name)
		{
			Id = name;
		}

		public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();
		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
	}
}
