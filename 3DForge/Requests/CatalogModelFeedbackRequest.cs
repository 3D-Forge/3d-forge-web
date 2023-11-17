using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
    public class CatalogModelFeedbackRequest
    {
        [Required]
        public int OrderedModelId { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        [MaxLength(512)]
        public string Text { get; set; }
        [MaxLength(512)]
        public string Pros { get; set; }
        [MaxLength(512)]
        public string Cons { get; set; }
    }
}
