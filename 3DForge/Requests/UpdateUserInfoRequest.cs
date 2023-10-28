using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend3DForge.Requests
{
    public class UpdateUserInfoRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime? Birthday { get; set; }
        [Required]
        public string? Sex { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Firstname { get; set; }
        [Required]
        public string? Midname { get; set; }
        [Required]
        public string? Lastname { get; set; }
        [Required]
        public string? Region { get; set; }
        [Required]
        public string? CityRegion { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Street { get; set; }
        [Required]
        public string? House { get; set; }
        [Required]
        public string? Apartment { get; set; }
        [Required]
        public string? DepartmentNumber { get; set; }
        [Required]
        public string? DeliveryType { get; set; }
    }
}
