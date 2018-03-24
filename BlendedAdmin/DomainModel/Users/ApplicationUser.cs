using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BlendedAdmin.DomainModel.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string TenantId { get; set; }
    }
}
