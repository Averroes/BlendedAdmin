using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlendedAdmin.DomainModel.Items;
using BlendedAdmin.Data;
using BlendedAdmin.DomainModel.Variables;
using BlendedAdmin.DomainModel.Environments;

namespace BlendedAdmin.DomainModel
{
    public interface IDomainContext
    {
        IItemRepository Items { get; }
        IVariableRepository Variables { get; set; }
        IEnvironmentRepository Environments { get; set; }

        Task SaveAsync();
    }

    public class DomainContext : IDomainContext
    {
        private ApplicationDbContext _dbContext;
        public IItemRepository Items { get; set; }
        public IVariableRepository Variables { get; set; }
        public IEnvironmentRepository Environments { get; set; }

        public DomainContext(ApplicationDbContext dbContext, IItemRepository itemRepository, IVariableRepository variables, IEnvironmentRepository environments)
        {
            this._dbContext = dbContext;
            this.Items = itemRepository;
            this.Variables = variables;
            this.Environments = environments;
        }

        public async Task SaveAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }
    }
}
