using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderRecord { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Firstname { get; set; }
        public string Midname { get; set; }
        public string? Lastname { get; set; }
        public string? Region { get; set; }
        public string? CityRegion { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Apartment { get; set; }
        public string? DepartmentNumber { get; set; }
        public string? DeliveryType { get; set; }
        public string? BillOfLading { get; set; }

        public ICollection<OrderedModel> OrderedModels { get; set; } = new List<OrderedModel>();
        public ICollection<OrderStatusOrder> OrderStatusOrders { get; set; } = new List<OrderStatusOrder>();
    }
}
