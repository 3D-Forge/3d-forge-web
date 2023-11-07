using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class LoginRequest
	{
		[Required]
		[StringLength(480, MinimumLength = 1)]
		public string LoginOrEmail { get; set; }
		[Required]
		[StringLength(480, MinimumLength = 6)]
		public string Password { get; set; }
	}
}
