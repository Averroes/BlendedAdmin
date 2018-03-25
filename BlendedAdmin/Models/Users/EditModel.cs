using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Users
{
    public class EditModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
