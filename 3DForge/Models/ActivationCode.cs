using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class ActivationCode
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int UserId { get; set; }
		public User User { get; set; }
		[Required]
		public string Code { get; set; }
		[Required]
		public string Action { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public DateTime Expires { get; set; }
	}
}
