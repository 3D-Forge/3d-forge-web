﻿using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class Cart
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public int UserId { get; set; }
		public User User { get; set; }

		public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
	}
}
