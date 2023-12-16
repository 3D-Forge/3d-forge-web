using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class CreateThreadRequest
	{
		[Required]
		public string ForumName { get; set; }
		public CreatePostRequest? Post { get; set; }
	}
}
