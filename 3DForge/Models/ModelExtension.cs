using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class ModelExtension
    {
        [Key]
        public int ModelExtensionId { get; set; }
        [Required]
        public string ModelExtensionName { get; set; }
    }
}
