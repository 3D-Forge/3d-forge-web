using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class Publish3DModelRequest
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MaxLength(500)]
		public string Description { get; set; }
		[Required]
		public float Depth { get; set; }
		[MaxLength(25)]
		public string[]? Keywords { get; set; }
		[Required]
		[MinLength(1)]
		[MaxLength(3)]
		public int[] Categories { get; set; }
	}
}
