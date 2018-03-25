using BlendedAdmin.Data;
using BlendedAdmin.DomainModel.Users;
using BlendedAdmin.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BlendedAdmin.Infrastructure
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        private ITenantService _tenantService;

        public ApplicationUserStore(ApplicationDbContext context, ITenantService tenantService) : base(context)
        {
            this._tenantService = tenantService;
        }

        public ApplicationUserStore(ApplicationDbContext context, ITenantService tenantService, IdentityErrorDescriber describer = null) : base(context, describer)
        {
            this._tenantService = tenantService;
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TenantId = this._tenantService.GetCurrentTenantId();
            return base.CreateAsync(user, cancellationToken);
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Context.Set<ApplicationUser>()
                .Where(u => u.Email.ToUpper() == email.ToUpper() && u.TenantId == this._tenantService.GetCurrentTenantId())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Context.Set<ApplicationUser>()
                .Where(u => u.UserName.ToUpper() == userName.ToUpper() && u.TenantId == this._tenantService.GetCurrentTenantId())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IList<Claim>>(new List<Claim>());
        }

        public override Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }
    }
}
