using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests.Administrate
{
    public class ModelCategoryRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
