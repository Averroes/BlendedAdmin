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
        private IOptions<HostingOptions> _settings;
        private IUrlService _urlService;

        public TenantService(
            IHttpContextAccessor httpContextAccessor, 
            IOptions<HostingOptions> settings,
            IUrlService urlService)
        {
            _httpContextAccessor = httpContextAccessor;
            _settings = settings;
            _urlService = urlService;
        }

        public string GetCurrentTenantId()
        {
            if (_settings.Value.MultiTenants)
            {
                return _urlService.GetTenant();
            }
            return null;
        }
    }
}
