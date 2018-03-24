using System;
using System.Threading.Tasks;
using BlendedAdmin.DomainModel.Environments;
using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Users
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; internal set; }
    }
}
