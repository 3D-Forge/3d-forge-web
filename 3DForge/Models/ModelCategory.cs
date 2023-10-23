using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class ModelCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ModelCategoryName { get; set; }

        public ICollection<CatalogModel> CatalogCategoryModels { get; set; } = new List<CatalogModel>();
    }
}
