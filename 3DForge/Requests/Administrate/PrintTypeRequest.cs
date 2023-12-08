using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests.Administrate
{
    public class PrintTypeRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
