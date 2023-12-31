﻿using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class Update3DModelRequest
	{
		[MaxLength(50)]
		public string? Name { get; set; }
		[MaxLength(500)]
		public string? Description { get; set; }
		public float? Depth { get; set; }
		[MaxLength(25)]
		public string[]? Keywords { get; set; }
		[MinLength(1)]
		[MaxLength(3)]
		public int[]? Categories { get; set; }
	}
}
