using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
	public class OrderRequest
	{
		[Required]
		public int CartId { get; set; }
		[Required]
		public string Firstname { get; set; }
		[Required]
		public string Midname { get; set; }
		public string? Lastname { get; set; }
		public string? Country { get; set; }
		public string? Region { get; set; }
		public string? City { get; set; }
		public string? CityRegion { get; set; }
		public string? Street { get; set; }
		public string? House { get; set; }
		public string? Apartment { get; set; }
		public int? DepartmentNumber { get; set; }
		public int? PostMachineNumber { get; set; }
	}
}
