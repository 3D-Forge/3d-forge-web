using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class UpdatePostRequest
	{
		[Required]
		public int PostId { get; set; }
		[Required]
		public string Text { get; set; }
	}
}
