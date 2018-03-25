using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Users
{
    public class LogInModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
