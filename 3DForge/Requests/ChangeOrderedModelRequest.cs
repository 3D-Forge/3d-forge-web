using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class ChangeOrderedModelRequest
	{
		[Required]
		public int Id { get; set; }
		public string? PrintType { get; set; }
		public string? PrintMaterial { get; set; }
		public string? PrintColor { get; set; }
		[Range(5, 100)]
		public float? Depth { get; set; }
		public double? Scale { get; set; } = 1.0f;
	}
}
