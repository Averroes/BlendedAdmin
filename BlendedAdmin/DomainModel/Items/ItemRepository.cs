using BlendedAdmin.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlendedAdmin.Models;

namespace BlendedAdmin.DomainModel.Items
{
    public interface IItemRepository
    {
        Task<Item> Get(int id);
        Task<Item> GetByName(string name);
        Task<List<Item>> GetAll();
        void Save(Item item);
        Task<List<string>> GetAllCategories();
        Task Delete(int value);
    }

    public class ItemRepository : IItemRepository
    {
        private ApplicationDbContext _dbContext;
        public ItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Item> Get(int id)
        {
            return await _dbContext.Items
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Item> GetByName(string name)
        {
            return _dbContext.Items
                .FirstOrDefaultAsync(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public Task<List<Item>> GetAll()
        {
            return _dbContext.Items.ToListAsync();
        }

        public void Save(Item item)
        {
            if (item.Id > 0)
                _dbContext.Update(item);
            else
                _dbContext.Add(item);
        }

        public void Delete(Item item)
        {
            _dbContext.Remove(item);
        }

        public async Task Delete(int id)
        {
            Item item = await this.Get(id);
            if (item != null)
                this._dbContext.Items.Remove(item);
        }

        public Task<List<string>> GetAllCategories()
        {
            return _dbContext.Items
                .Select(x => x.Category)
                .Where(x => string.IsNullOrEmpty(x) == false)
                .Distinct().ToListAsync();
        }
    }
}
