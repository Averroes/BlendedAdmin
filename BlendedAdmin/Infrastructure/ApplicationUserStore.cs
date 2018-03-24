using BlendedAdmin.Data;
using BlendedAdmin.DomainModel.Users;
using BlendedAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlendedAdmin.Infrastructure
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public string TenantId { get; set; }

        public ApplicationUserStore(DbContext context) : base(context)
        {
        }

        public ApplicationUserStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TenantId = this.TenantId;
            return base.CreateAsync(user, cancellationToken);
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Context.Set<ApplicationUser>().Where(u => u.Email.ToUpper() == email.ToUpper() && u.TenantId == this.TenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Context.Set<ApplicationUser>().Where(u => u.UserName.ToUpper() == userName.ToUpper() && u.TenantId == this.TenantId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
