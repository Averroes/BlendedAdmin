using BlendedAdmin.DomainModel.Tenants;
using BlendedAdmin.Infrastructure;
using BlendedAdmin.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BlendedAdmin
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        private IServiceScopeFactory _serviceScopeFactory;
        private IOptions<HostingOptions> _hostingOptions;
        private IMemoryCache _memoryCache;
        private IUrlService _urlService;
        private ILogger<TenantMiddleware> _logger { get; }

        public TenantMiddleware(
            RequestDelegate next,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<HostingOptions> hostingOptions,
            IMemoryCache memoryCache,
            IUrlService urlService,
            ILogger<TenantMiddleware> logger)
        {
            _next = next;

            _serviceScopeFactory = serviceScopeFactory;
            _hostingOptions = hostingOptions;
            _memoryCache = memoryCache;
            _urlService = urlService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_hostingOptions?.Value?.MultiTenants == false)
            {
                await this._next(context);
                return;
            }

            string tenantId = _urlService.GetTenant();
            if (tenantId == "register")
            {
                if (context.Request.Path == "/tenants/register" ||
                    context.Request.Path == "/tenants/registrationconfirmation")
                {
                    await this._next(context);
                    return;
                } 
                else
                {
                    context.Response.Redirect("/tenants/register");
                    return;
                }
            }
            else
            {
                if (context.Request.Path == "/tenants/register" ||
                    context.Request.Path == "/tenants/registrationconfirmation")
                {
                    context.Response.Redirect("/");
                    return;
                }
            }

            string tenantCacheKey = "tenant_" + tenantId;
            Tenant tenant = null;
            if (!_memoryCache.TryGetValue(tenantCacheKey, out tenant))
            {
                tenant = await GetTenant(tenantId);
                if (tenant == null)
                {
                    context.Response.StatusCode = 404;
                    _logger.LogInformation("Tenant not found: " + tenantId);
                    await context.Response.WriteAsync("Page not found");
                    return;
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                        //.SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                _memoryCache.Set(tenantCacheKey, tenant, cacheOptions);
            }
            await this._next(context);
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

    public static class TenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenant(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TenantMiddleware>();
        }
    }
}
