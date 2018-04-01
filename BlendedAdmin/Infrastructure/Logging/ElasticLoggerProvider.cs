using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BlendedAdmin.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BlendedAdmin.Infrastructure.Logging
{
    [ProviderAlias("MySql")]
    public class ElasticLoggerProvider : ILoggerProvider
    {
        private IOptions<ElasticLoggerOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        private ITenantService _tenantService;
        private Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        public ElasticLoggerProvider(
            IOptions<ElasticLoggerOptions> options, 
            IHttpContextAccessor httpContextAccessor,
            IServiceScopeFactory serviceScopeFactory,
            ITenantService tenantService)
        {
            this._options = options;
            this._httpContextAccessor = httpContextAccessor;
            this._serviceScopeFactory = serviceScopeFactory;
            this._tenantService = tenantService;
        }

        public ILogger CreateLogger(string categoryName)
        {
            ILogger logger;
            if (_loggers.TryGetValue(categoryName, out logger))
                return logger;

            logger = new ElasticLogger(categoryName, _options, _httpContextAccessor, _serviceScopeFactory, _tenantService);
            _loggers[categoryName] = logger;
            return logger;
        }

        public void Dispose()
        {
        }
    }
}