using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class Keyword
    {
        [Key]
        public int KeywordId { get; set; }
        [Required]
        public string KeywordName { get; set; }
    }
}
