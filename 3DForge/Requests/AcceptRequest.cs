using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class AcceptRequest
	{
		[Required]
		public bool Accepted { get; set; }
		public string? Message {  get; set; }
	}
}
