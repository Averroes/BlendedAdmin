using BlendedAdmin.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.DomainModel.Variables
{
    public interface IVariableRepository
    {
        Task<Variable> GetById(int id);
        Task<Variable> GetByName(string name);
        Task<List<Variable>> GetAll();
        void Save(Variable variable);
        Task Delete(int id);
    }

    public class VariableRepository : IVariableRepository
    {
        private ApplicationDbContext _dbContext;
        public VariableRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Variable> GetById(int id)
        {
            return await _dbContext.Variables
                .Include(x => x.Values)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Variable> GetByName(string name)
        {
            return await _dbContext.Variables
                .Include(x => x.Values)
                .FirstOrDefaultAsync(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<List<Variable>> GetAll()
        {
            return await _dbContext.Variables
                .Include(x => x.Values)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task Delete(int id)
        {
            var variable = await GetById(id);
            _dbContext.Entry(variable)
                .Collection(b => b.Values)
                .Load();
            _dbContext.VariablesEnvironments.RemoveRange(variable.Values);
            _dbContext.Variables.Remove(variable);
        }

        public void Save(Variable variable)
        {
            if (variable.Id > 0)
                _dbContext.Update(variable);
            else
                _dbContext.Add(variable);
        }
    }
}
