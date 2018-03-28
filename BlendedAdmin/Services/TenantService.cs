using BlendedAdmin.DomainModel;
using BlendedAdmin.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Services
{
    public interface ITenantService
    {
        string GetCurrentTenantId();
    }

    public class TenantService : ITenantService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<BlendedSettings> _settings;

        public TenantService(IHttpContextAccessor httpContextAccessor, IOptions<BlendedSettings> settings)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._settings = settings;
        }

        public string GetCurrentTenantId()
        {
            if (_settings.Value.MultiTenants)
            {
                var request = _httpContextAccessor.HttpContext.Request;
                if (string.IsNullOrWhiteSpace(request.Host.Host) == false)
                {
                    var subDomains = request.Host.Host.Split('.');
                    return subDomains[0].Trim().ToLower();
                }
            }
            return null;
        }
    }
}
