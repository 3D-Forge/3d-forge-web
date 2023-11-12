using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
    public class ChangeEmailRequest
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
