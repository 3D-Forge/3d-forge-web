using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests.Administrate
{
    public class PrintExtensionRequest
    {
        [Required]
        [MaxLength(8)]
        public string Name { get; set; }
    }
}
