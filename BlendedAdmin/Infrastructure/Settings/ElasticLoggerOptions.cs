using Microsoft.Extensions.Logging;

namespace BlendedAdmin.Infrastructure
{
    public class ElasticLoggerOptions
    {
        public LogLevel? LogLevel { get; set; }
        public string Url { get; set; }
    }

}