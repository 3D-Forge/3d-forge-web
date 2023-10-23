using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class BannedWord
	{
		[Key]
		public int BannedWordId { get; set; }
		[Required]
		public string BannedWordName { get; set; }
	}
}
