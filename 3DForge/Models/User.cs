using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend3DForge.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Email { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? Birthday { get; set; }
        public string? Sex { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Midname { get; set; }
        public string? Lastname { get; set; }
        public string? Region { get; set; }
        public string? CityRegion { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Apartment { get; set; }
        public string? DepartmentNumber { get; set; }
        public string? DeliveryType { get; set; }
        [Required]
        public bool Blocked { get; set; }
        [Required]
        public bool CanAdministrateForum { get; set; }
        [Required]
        public bool CanRetrieveDelivery { get; set; }
        [Required]
        public bool CanModerateCatalog { get; set; }
        [Required]
        public bool CanAdministrateSystem { get; set; }
        [Required]
        public bool IsActivated { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }

        public ICollection<ActivationCode> ActivationCodes { get; set; } = new List<ActivationCode>();
        public ICollection<ForumThread> ForumThreads { get; set; } = new List<ForumThread>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<CatalogModel> CatalogModels { get; set; } = new List<CatalogModel>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
