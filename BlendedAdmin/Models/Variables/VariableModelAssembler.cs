using System;
using System.Collections.Generic;
using System.Linq;
using BlendedAdmin.DomainModel.Variables;
using Environment = BlendedAdmin.DomainModel.Environments.Environment;

namespace BlendedAdmin.Models.Variables
{
    public class VariableModelAssembler
    {
        public VariableModelAssembler()
        {
        }

        public VariableModel ToModel(Variable variable, List<Environment> environments)
        {
            VariableModel model = new VariableModel();
            model.Values = new List<VariableValueModel>();
            model.Id = variable.Id;
            model.Name = variable.Name;

            VariableValueModel defaultValue = new VariableValueModel();
            defaultValue.Value = variable.Value;
            model.Values.Add(defaultValue);

            foreach (BlendedAdmin.DomainModel.Environments.Environment environment in environments)
            {
                VariableValueModel value = new VariableValueModel();
                value.EnvironmentId = environment.Id;
                value.Environment = environment.Name;
                value.Color = environment.Color;
                value.Value = (variable.Values ?? new List<VariableEnvironment>())
                    .FirstOrDefault(x => x.Environment == environment)?.Value;
                model.Values.Add(value);
            }

            return model;
        }

        public List<VariableModel> ToModel(List<Variable> variable, List<Environment> environments)
        {
            return variable.Select(x => ToModel(x, environments)).ToList();
        }

        public void ApplyModel(VariableModel model, Variable variable, List<Environment> environments)
        {
            //variable.Id = model.Id;
            variable.Name = model.Name;
            variable.Value = (model.Values ?? new List<VariableValueModel>()).FirstOrDefault(x => x.EnvironmentId == 0)?.Value;
            if (variable.Values == null) variable.Values = new List<VariableEnvironment>();
            foreach (var valueModel in model.Values ?? new List<VariableValueModel>())
            {
                var value = variable.Values.FirstOrDefault(x => x.Environment.Id == valueModel.EnvironmentId);
                if (value == null)
                {
                    var environment = environments.Find(x => x.Id == valueModel.EnvironmentId);
                    if (environment != null)
                    {
                        value = new VariableEnvironment();
                        value.Environment = environment;
                        value.Variable = variable;
                        value.Value = valueModel.Value;
                        variable.Values.Add(value);
                    }
                }
                else
                {
                    value.Value = valueModel.Value;
                }
            }
        }
    }
}
