using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend3DForge.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Birthday { get; set; }
        public string? Sex { get; set; } = null;
        [Phone]
        public string? PhoneNumber { get; set; } = null;
        public string? Firstname { get; set; } = null;
        public string? Midname { get; set; } = null;
        public string? Lastname { get; set; } = null;
        public string? Region { get; set; } = null;
        public string? CityRegion { get; set; } = null;
        public string? City { get; set; } = null;
        public string? Street { get; set; } = null;
        public string? House { get; set; } = null;
        public string? Apartment { get; set; } = null;
        public string? DepartmentNumber { get; set; } = null;
        public string? DeliveryType { get; set; } = null;
        [Required]
        public bool Blocked { get; set; }
        [Required]
        public bool CanAdministrateForum { get; set; }
        [Required]
        public bool CanRetrieveDelivery { get; set; }
        [Required]
        public bool CanModeratеCatalog { get; set; }
        [Required]
        public bool CanAdministrateSystem { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
    }
}
