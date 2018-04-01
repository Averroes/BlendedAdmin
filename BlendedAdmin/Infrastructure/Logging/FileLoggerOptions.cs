using Microsoft.Extensions.Logging;

namespace BlendedAdmin.Infrastructure.Logging
{
    public class FileLoggerOptions
    {
        public LogLevel? LogLevel { get; set; }
        public string FilePath { get; set; }
    }

}