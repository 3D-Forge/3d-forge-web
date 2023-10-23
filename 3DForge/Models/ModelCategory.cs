using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class ModelCategory
    {
        [Key]
        public int ModelCategoryId { get; set; }
        [Required]
        public string ModelCategoryName { get; set; }
    }
}
