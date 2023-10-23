using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintType
    {
        [Key]
        public int PrintTypeId { get; set; }
        [Required]
        public string PrintTypeName { get; set; }
    }
}
