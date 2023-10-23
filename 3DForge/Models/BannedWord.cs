using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class BannedWord
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string BannedWordName { get; set; }
	}
}
