using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Users
{
    public class ChangeMyPassowrdModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public bool Succeeded { get; internal set; }
    }
}
