using BlendedAdmin.Data;
using BlendedAdmin.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.DomainModel.Environments
{
    public interface IEnvironmentRepository
    {
        Task<List<Environments.Environment>> GetAll();
        Task<Environment> Get(int environmentId);
        Task<Environments.Environment> GetByName(string name);
        void Delete(Environments.Environment environment);
        void Save(Environments.Environment environment);
        Task<int> GetNextIndex();
    }

    public class EnvironmentRepository : IEnvironmentRepository
    {
        private ApplicationDbContext _dbContext;
        private ITenantService _tenantService;

        public EnvironmentRepository(ApplicationDbContext dbContext, ITenantService tenantService)
        {
            _dbContext = dbContext;
            _tenantService = tenantService;
        }

        public async Task<Environments.Environment> Get(int id)
        {
            return await _dbContext.Environments.Where(x => x.TenantId == _tenantService.GetCurrentTenantId()).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Environments.Environment> GetByName(string name)
        {
            return await _dbContext.Environments.Where(x => x.TenantId == _tenantService.GetCurrentTenantId()).FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Environments.Environment>> GetAll()
        {
            return await _dbContext.Environments.Where(x => x.TenantId == _tenantService.GetCurrentTenantId()).OrderBy(x => x.Index).ToListAsync();
        }

        public void Delete(Environments.Environment environment)
        {
            _dbContext.Entry(environment)
                .Collection(b => b.Variables)
                .Load();
            _dbContext.VariablesEnvironments.RemoveRange(environment.Variables);
            _dbContext.Environments.Remove(environment);
        }

        public void Save(Environments.Environment environment)
        {
            if (environment.Id > 0)
            {
                _dbContext.Update(environment);
            }
            else
            {
                environment.TenantId = _tenantService.GetCurrentTenantId();
                _dbContext.Add(environment);
            }
        }

        public async Task<int> GetNextIndex()
        {
            var environments =  await GetAll();
            if (environments.Count == 0)
                return 0;
            else
                return environments.Max(x => x.Index) + 1;
        }
    }
}
