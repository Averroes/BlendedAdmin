using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Users
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
