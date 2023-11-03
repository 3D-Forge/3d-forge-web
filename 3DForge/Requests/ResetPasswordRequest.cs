using System.ComponentModel.DataAnnotations;

namespace Backend3DForge.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
        public string? OldPassword { get; set; }
        public string? Token { get; set; }
    }
}
