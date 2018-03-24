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
        Task<List<ApplicationUser>> GetAll(string tenantId);
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

        public async Task<List<ApplicationUser>> GetAll(string tenantId)
        {
            return await _dbContext.Users.Where(x => x.TenantId == tenantId).ToListAsync();
        }
    }
}
