using BlendedAdmin.DomainModel.Tenants;
using BlendedAdmin.Infrastructure;
using BlendedAdmin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlendedAdmin
{
    public class ValidateTenantFilter : ActionFilterAttribute
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<HostingOptions> _hostingOptions;
        private IMemoryCache _memoryCache;
        private IUrlService _urlService;
        private ILogger<ValidateTenantFilter> _logger { get; }

        public ValidateTenantFilter(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<HostingOptions> hostingOptions,
            IMemoryCache memoryCache,
            IUrlService urlService,
            ILogger<ValidateTenantFilter> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hostingOptions = hostingOptions;
            _memoryCache = memoryCache;
            _urlService = urlService;
            _logger = logger;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_hostingOptions?.Value?.MultiTenants == false)
                return;

            string tenantId = _urlService.GetTenant();
            if (tenantId == "x")
                return;
            string tenantCacheKey = "tenant_" + tenantId;
            Tenant tenant = null;
            if (!_memoryCache.TryGetValue(tenantCacheKey, out tenant))
            {
                tenant = await GetTenant(tenantId);
                if (tenant == null)
                {
                    _logger.LogInformation("Tenant does not exists: " + tenantId);
                    context.Result = new ContentResult { Content = "Page not found.", StatusCode = 404 };
                    return;
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    //.SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                _memoryCache.Set(tenantCacheKey, tenant, cacheOptions);
            }
        }

        public async Task<Tenant> GetTenant(string tenantId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
                var tenant = await tenantRepository.Get(tenantId);
                return tenant;
            }
        }
    }
}
