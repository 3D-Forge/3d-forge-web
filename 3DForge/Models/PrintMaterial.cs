using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintMaterial
    {
        [Key]
        public int PrintMaterialId { get; set; }
        [Required]
        public string PrintMaterialName { get; set; }
    }
}
