using Backend3DForge.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class ModelExtension : ITableKey<string>
	{
		[Key]
		public string Id { get; set; }

		public ModelExtension() { }

		public ModelExtension(string name)
		{
			Id = name;
		}

		public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();

		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
	}
}
