using BlendedAdmin.Data;
using BlendedAdmin.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.DomainModel.Users
{
    public interface IUserRepository
    {
        Task<ApplicationUser> Get(string id);
        Task<List<ApplicationUser>> GetAll();
        void Save(ApplicationUser user, string tenantId);
    }

    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _dbContext;
        private ITenantService _tenantService;

        public UserRepository(ApplicationDbContext dbContext, ITenantService tenantService)
        {
            _dbContext = dbContext;
            _tenantService = tenantService;
        }

        public async Task<ApplicationUser> Get(string id)
        {
            return await _dbContext.Users
                .Where(x => x.TenantId == _tenantService.GetCurrentTenantId())
                .FirstAsync(x => x.Id == id);
        }

        public async Task<List<ApplicationUser>> GetAll()
        {
            return await _dbContext.Users
                .Where(x => x.TenantId == _tenantService.GetCurrentTenantId())
                .ToListAsync();
        }

        public void Save(ApplicationUser user, string tenantId)
        {
            user.TenantId = tenantId;
            _dbContext.Users.Add(user);
        }
    }
}
