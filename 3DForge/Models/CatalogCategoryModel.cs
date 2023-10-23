using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class CatalogCategoryModel
    {
        [Key]
        public int CatalogCategoryModelId { get; set; }
        [Required]
        public int CatalogModelId { get; set; }
        [Required]
        public int ModelCategoryId { get; set; }
    }
}
