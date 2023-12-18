﻿using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class CartUpdateRequest
	{
		[Required]
		public int OrderedModelId { get; set; }
		[Required]
		public int Pieces { get; set; }
		[Required]
		[Range(5, 100)]
		public float Depth { get; set; }
		[Required]
		public float Scale { get; set; } = 1.0f;
		[Required]
		public int ColorId { get; set; }
		[Required]
		public string PrintTypeName { get; set; }
		[Required]
		public string PrintMaterialName { get; set; }
	}
}
