using Backend3DForge.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class PrintMaterialColor : ITableKey<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        public byte Red { get; set; }
        [Required] 
        public byte Green { get; set; }
        [Required] 
        public byte Blue { get; set; }
        [Required]
        public string PrintMaterialId { get; set; }
        public PrintMaterial PrintMaterial { get; set; }

        public float Cost { get; set; } = 0;

        public string Hex => $"#{Red:X2}{Green:X2}{Blue:X2}";
        public string RGB => $"{Red},{Green},{Blue}";

        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
