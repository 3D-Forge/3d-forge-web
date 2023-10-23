using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PrintTypeName { get; set; }

        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
