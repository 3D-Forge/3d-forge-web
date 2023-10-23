using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class ForumThread
	{
		[Key]
		public int ForumThreadId { get; set; }
		[Required]
		public string ForumName { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public int UserId { get; set; }
	}
}
