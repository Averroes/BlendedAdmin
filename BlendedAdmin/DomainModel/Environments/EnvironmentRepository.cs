using BlendedAdmin.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.DomainModel.Environments
{
    public interface IEnvironmentRepository
    {
        Task<List<Environments.Environment>> GetAll();
        void Delete(Environments.Environment environment);
        void Save(Environments.Environment environment);
        Task<Environment> Get(int environmentId);
        Task<Environments.Environment> GetByName(string name);
        Task<int> GetNextIndex();
    }

    public class EnvironmentRepository : IEnvironmentRepository
    {
        private ApplicationDbContext _dbContext;
        public EnvironmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Environments.Environment> Get(int id)
        {
            return await _dbContext.Environments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Environments.Environment> GetByName(string name)
        {
            return await _dbContext.Environments.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Environments.Environment>> GetAll()
        {
            return await _dbContext.Environments.OrderBy(x => x.Index).ToListAsync();
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
                _dbContext.Update(environment);
            else
                _dbContext.Add(environment);
        }

        public async Task<int> GetNextIndex()
        {
            return (await _dbContext.Environments.MaxAsync(x => x.Index)) + 1;
        }
    }
}
