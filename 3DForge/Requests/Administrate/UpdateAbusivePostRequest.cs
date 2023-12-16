using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests.Administrate
{
	public class UpdateAbusivePostRequest
	{
		[Required]
		public int PostId { get; set; }
		[Required]
		public bool ContainsAbusive { get; set; }
	}
}
