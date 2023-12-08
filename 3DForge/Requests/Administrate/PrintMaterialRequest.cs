using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests.Administrate
{
    public class PrintMaterialRequest
    {
        [Required]
        public string MaterialName { get; set; }
        [Required]
        public string TechnologyName { get; set; }
        [Required]
        public float Density { get; set; }
        [Required]
        public float Cost { get; set; }
    }
}
