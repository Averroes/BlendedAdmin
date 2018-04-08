using BlendedAdmin.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlendedAdmin.Models;
using BlendedAdmin.Services;

namespace BlendedAdmin.DomainModel.Tenants
{
    public interface ITenantRepository
    {
        Task<Tenant> Get(string id);
        void Save(Tenant item);
    }

    public class TenantRepository : ITenantRepository
    {
        private ApplicationDbContext _dbContext;
        private ITenantService _tenantService;

        public TenantRepository(ApplicationDbContext dbContext, ITenantService tenantService)
        {
            _dbContext = dbContext;
            _tenantService = tenantService;
        }

        public async Task<Tenant> Get(string id)
        {
            return await _dbContext
                .Tenants
                .FirstOrDefaultAsync(x => string.Equals(x.Id, id, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Save(Tenant item)
        {
            item.CreatedDate = DateTime.UtcNow;
            _dbContext.Tenants.Add(item);
        }
    }
}
