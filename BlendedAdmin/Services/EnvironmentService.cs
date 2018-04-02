using BlendedAdmin.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Services
{
    public interface IEnvironmentService
    {
        Task<List<Environment>> GetAll();
        Task<Environment> GetCurrentEnvironment();
    }

    public class EnvironmentService : IEnvironmentService
    {
        private IDomainContext _domainContext;
        private IUrlService _urlService;

        public EnvironmentService(IDomainContext domainContext, IUrlService urlService)
        {
            this._domainContext = domainContext;
            this._urlService = urlService;
        }

        public async Task<List<Environment>> GetAll()
        {
            return (await this._domainContext.Environments.GetAll()).Select(x => 
            {
                return new Environment {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color
                };
            }).ToList();
        }

        public async Task<Environment> GetCurrentEnvironment()
        {
            var allEnvironments = await GetAll();
            var selectedEnvironmentName = _urlService.GetEnvironment();
            var selectedEnvironment = allEnvironments.FirstOrDefault(x => x.Name == selectedEnvironmentName);
            if (selectedEnvironment != null)
                return selectedEnvironment;
            if (allEnvironments.Count > 0)
                return allEnvironments[0];
            return new Environment() { Id = 0, Name = "Default" };
        }
    }

    public class Environment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
