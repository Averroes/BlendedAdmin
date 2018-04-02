using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BlendedAdmin.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace BlendedAdmin.Infrastructure.Logging
{
    [ProviderAlias("MySql")]
    public class ElasticLoggerProvider : ILoggerProvider
    {
        private IOptions<ElasticLoggerOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        private IUrlService _urlService;
        private ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

        public ElasticLoggerProvider(
            IOptions<ElasticLoggerOptions> options, 
            IHttpContextAccessor httpContextAccessor,
            IServiceScopeFactory serviceScopeFactory,
            IUrlService urlService)
        {
            this._options = options;
            this._httpContextAccessor = httpContextAccessor;
            this._serviceScopeFactory = serviceScopeFactory;
            this._urlService = urlService;
        }

        public ILogger CreateLogger(string categoryName)
        {
            ILogger logger;
            if (_loggers.TryGetValue(categoryName, out logger))
                return logger;

            logger = new ElasticLogger(categoryName, _options, _httpContextAccessor, _serviceScopeFactory, _urlService);
            _loggers[categoryName] = logger;
            return logger;
        }

        public void Dispose()
        {
        }
    }
}