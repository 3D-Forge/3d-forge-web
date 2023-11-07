﻿using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
	public class OrderStatus
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string OrderStatusName { get; set; }

		public ICollection<OrderStatusOrder> OrderStatusOrders { get; set; } = new List<OrderStatusOrder>();
	}
}
