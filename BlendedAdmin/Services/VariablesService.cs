using BlendedAdmin.DomainModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Services
{
    public interface IVariablesService
    {
        Task<Dictionary<string, object>> GetVariables();
    }

    public class VariablesService : IVariablesService
    {
        private IEnvironmentService _environmentService;
        private IDomainContext _domainContext;

        public VariablesService(IDomainContext domainContext, IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
            _domainContext = domainContext;
        }

        public async Task<Dictionary<string, object>> GetVariables()
        {
            Dictionary<string, object> variablesValues = new Dictionary<string, object>();

            var environment = await _environmentService.GetCurrentEnvironment();
            var variables = await _domainContext.Variables.GetAll();
            
            foreach (var variable in variables)
            {
                var value = variable.Values.FirstOrDefault(x => x.Environment.Id == environment.Id);
                if (value != null && string.IsNullOrEmpty(value.Value) == false)
                    variablesValues[variable.Name] = value.Value;
                else
                    variablesValues[variable.Name] = variable.Value;
            }

            return variablesValues;
        }
    }
}
