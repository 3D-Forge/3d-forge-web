using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class ModelPicture
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long Size { get; set; }
        [Required]
        public int CatalogModelId { get; set; }
        public CatalogModel CatalogModel { get; set; }
        [Required]
        public DateTime Uploaded { get; set; }
    }
}
