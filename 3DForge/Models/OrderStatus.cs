using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Models
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        [Required]
        public string OrderStatusName { get; set; }
    }
}
