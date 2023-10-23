using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintExtension
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PrintExtensionName { get; set; }

        public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();
        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
