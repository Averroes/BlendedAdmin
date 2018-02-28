using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlendedAdmin.Models.Items
{
    [Bind(Prefix = "model")]
    public class ItemEditModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
    }
}
