using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string KeywordName { get; set; }

        public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>(); 
    }
}
