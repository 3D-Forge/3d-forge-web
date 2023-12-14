using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class Post
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int ForumThreadId { get; set; }
		public ForumThread ForumThread { get; set; }
		[Required]
		public string PostText { get; set; }
		[Required]
		public DateTime CreateAt { get; set; }
		public int? UserId { get; set; }
		public User? User { get; set; }
		[Required]
		public bool ContainsAbuseContent { get; set; }
		public int? ReplayPostId { get; set; }
		public Post? ReplayPost { get; set; }
		[Required]
		public DateTime EditedAt { get; set; }

		public ICollection<Post> Replays { get; set; } = new List<Post>();
	}
}
