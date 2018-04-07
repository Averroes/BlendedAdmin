using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlendedAdmin.DomainModel.Items;
using BlendedAdmin.Data;
using BlendedAdmin.DomainModel.Variables;
using BlendedAdmin.DomainModel.Environments;
using BlendedAdmin.DomainModel.Users;
using BlendedAdmin.DomainModel.Tenants;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlendedAdmin.DomainModel
{
    public interface IDomainContext
    {
        IUserRepository Users { get; }
        IItemRepository Items { get; }
        IVariableRepository Variables { get; set; }
        IEnvironmentRepository Environments { get; set; }
        ITenantRepository Tenants { get; set; }
        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransaction();
    }

    public class DomainContext : IDomainContext, IDisposable
    {
        private ApplicationDbContext _dbContext;

        public IUserRepository Users { get; set; }
        public IItemRepository Items { get; set; }
        public IVariableRepository Variables { get; set; }
        public IEnvironmentRepository Environments { get; set; }
        public ITenantRepository Tenants { get; set; }

        public DomainContext(ApplicationDbContext dbContext, IItemRepository itemRepository, IVariableRepository variables, IEnvironmentRepository environments, IUserRepository users, ITenantRepository tenantRepository)
        {
            this._dbContext = dbContext;
            this.Items = itemRepository;
            this.Variables = variables;
            this.Environments = environments;
            this.Users = users;
            this.Tenants = tenantRepository;
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await this._dbContext.Database.BeginTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._dbContext?.Dispose();
        }
    }
}
