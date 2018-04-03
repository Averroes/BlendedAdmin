using Microsoft.Extensions.Logging;

namespace BlendedAdmin.Infrastructure
{
    public class FileLoggerOptions
    {
        public LogLevel? LogLevel { get; set; }
        public string FilePath { get; set; }
    }
}