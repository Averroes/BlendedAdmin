using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BlendedAdmin.Models.Variables
{
    public class VariableModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IList<VariableValueModel> Values { get; set; } 
    }

    public class VariableValueModel
    {
        public int EnvironmentId { get; set; }
        public string Environment { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
    }
}
