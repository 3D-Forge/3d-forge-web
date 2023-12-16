using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class CreatePostRequest
	{
		[Required]
		public int ThreadId { get; set; }
		[Required]
		public string Text { get; set; }
		public int? ReplayPostId { get; set; }
	}
}
