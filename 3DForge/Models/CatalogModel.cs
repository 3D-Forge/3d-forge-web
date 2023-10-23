using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class CatalogModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int PrintExtensionId { get; set; }
        public PrintExtension PrintExtension { get; set; }
        [Required]
        public int ModelExtensionId { get; set; }
        public ModelExtension ModelExtension { get; set; }
        [Required]
		public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public DateTime Uploaded { get; set; }
        [Required]
        public DateTime? Publicized { get; set; }
        [Required]
        public float Height { get; set; }
        [Required]
        public float Width { get; set; }
        [Required]
        public float Depth { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public long FileSize { get; set; }

        public ICollection<ModelCategory> ModelCategoryes { get; set; } = new List<ModelCategory>();
        public ICollection<Keyword> Keywords { get; set; } = new List<Keyword>();
        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
    }
}
