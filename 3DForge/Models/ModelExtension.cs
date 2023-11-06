using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class ModelExtension
    {
        [Key]
        public string Name { get; set; }

        public ModelExtension() { }

        public ModelExtension(string name)
        {
            Name = name;
        }

		public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();

        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
