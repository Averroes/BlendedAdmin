using System;
using System.Threading.Tasks;
using BlendedAdmin.DomainModel.Environments;
using System.ComponentModel.DataAnnotations;

namespace BlendedAdmin.Models.Environments
{
    public class EnvironmentModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? Deleted { get; set; }
        public string Color { get; set; }
    }
}
