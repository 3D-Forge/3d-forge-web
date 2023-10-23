using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintMaterial
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PrintMaterialName { get; set; }

        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
