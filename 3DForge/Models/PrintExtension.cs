using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintExtension
    {
        [Key]
        public int PrintExtensionId { get; set; }
        [Required]
        public string PrintExtensionName { get; set; }
    }
}
