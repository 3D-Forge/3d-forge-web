using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class Post
	{
		[Key]
		public int PostId { get; set; }
		[Required]
		public int ForumThreadId { get; set; }
		[Required]
		public string PostText { get; set; }
		[Required]
		public DateTime CreateAt { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public bool ContainsAbuseContent { get; set; }
		[Required]
		public int NextPostId { get; set; }
		[Required]
		public DateTime EditedAt { get; set; }
	}
}
