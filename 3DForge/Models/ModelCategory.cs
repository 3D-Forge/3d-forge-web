using Backend3DForge.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class ModelCategory : ITableKey<int>
    {
		[Key]
		public int Id { get; set; }
		[Required]
		public string ModelCategoryName { get; set; }

		public ModelCategory() { }

		public ModelCategory(string name)
		{
			ModelCategoryName = name;
		}

		public ICollection<CatalogModel> CatalogCategoryModels { get; set; } = new List<CatalogModel>();
	}
}
