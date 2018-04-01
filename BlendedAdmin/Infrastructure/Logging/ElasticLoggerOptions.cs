using Microsoft.Extensions.Logging;

namespace BlendedAdmin.Infrastructure.Logging
{
    public class ElasticLoggerOptions
    {
        public LogLevel? LogLevel { get; set; }
        public string Url { get; set; }
    }

}