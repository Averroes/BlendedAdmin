using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.DomainModel.Variables
{
    public class Variable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }
        public IList<VariableEnvironment> Values { get; set; }
    }

    public class VariableEnvironment
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public BlendedAdmin.DomainModel.Variables.Variable Variable { get; set; }
        public BlendedAdmin.DomainModel.Environments.Environment Environment { get; set; }
    }
}
