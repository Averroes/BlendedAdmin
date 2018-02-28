using System.Collections.Generic;
using System.Linq;
using Environment = BlendedAdmin.DomainModel.Environments.Environment;

namespace BlendedAdmin.Models.Environments
{
    public class EnvironmentModelAssembler
    {
        public EnvironmentModel ToModel(Environment environment)
        {
            EnvironmentModel model = new EnvironmentModel();
            model.Id = environment.Id;
            model.Name = environment.Name;
            model.Color = environment.Color;
            return model;
        }

        public List<EnvironmentModel> ToModel(List<Environment> environments)
        {
            return environments.Select(x => ToModel(x)).ToList();
        }

        public void Apply(Environment environment, EnvironmentModel model)
        {
            environment.Name = model.Name;
            environment.Color = model.Color;
        }
    }
}
