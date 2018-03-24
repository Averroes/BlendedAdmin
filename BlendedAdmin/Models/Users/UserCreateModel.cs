using System;
using System.Threading.Tasks;
using BlendedAdmin.DomainModel.Environments;
using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Users
{
    public class UserCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
