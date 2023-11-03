using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class ModelExtension
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ModelExtensionName { get; set; }

        public ModelExtension() { }

        public ModelExtension(string modelExtensionName)
        {
            ModelExtensionName = modelExtensionName;
        }

		public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();

        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
