using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BlendedAdmin.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BlendedAdmin.Infrastructure.Logging
{
    [ProviderAlias("File")]
    public class FileLoggerProvider : ILoggerProvider
    {
        private IOptions<FileLoggerOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        private Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        public FileLoggerProvider(
            IOptions<FileLoggerOptions> options, 
            IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            ILogger logger;
            if (_loggers.TryGetValue(categoryName, out logger))
                return logger;

            logger = new FileLogger(categoryName, _options, _httpContextAccessor);
            _loggers[categoryName] = logger;
            return logger;
        }

        public void Dispose()
        {
        }
    }
}