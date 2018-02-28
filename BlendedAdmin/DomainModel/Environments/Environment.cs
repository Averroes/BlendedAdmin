using BlendedAdmin.DomainModel.Variables;
using System;
using System.Collections.Generic;

namespace BlendedAdmin.DomainModel.Environments
{
    public class Environment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }

        public IList<VariableEnvironment> Variables { get; set; }
    }
}
