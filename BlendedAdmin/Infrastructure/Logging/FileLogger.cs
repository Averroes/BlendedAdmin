using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using Microsoft.Extensions.Options;
using System.Text;

namespace BlendedAdmin.Infrastructure.Logging
{
    public class FileLogger : ILogger
    {
        private IOptions<FileLoggerOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;

        private LogLevel _logLevel;
        private string _filePath;
        private string _category;

        public FileLogger(
            string category,
            IOptions<FileLoggerOptions> options,
            IHttpContextAccessor httpContextAccessor)
        {
            _category = category;
            _options = options;
            _httpContextAccessor = httpContextAccessor;

            _logLevel = options.Value.LogLevel ?? LogLevel.None;
            _filePath = options.Value.FilePath ?? "Logs";
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                if (_options.Value.LogLevel <= logLevel)
                {
                    Directory.CreateDirectory(_filePath);

                    DateTime dateTime = DateTime.Now;
                    string fullFileName = Path.Combine(_filePath, $"log-{dateTime.Year:0000}_{dateTime.Month:00}_{dateTime.Day:00}.txt");
                    var logLine = new StringBuilder();
                    logLine.Append(dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
                    logLine.Append(" [");
                    logLine.Append(logLevel.ToString());
                    logLine.Append("] ");
                    logLine.Append(_category);
                    logLine.Append(": ");
                    logLine.Append(_httpContextAccessor.HttpContext?.User?.Identity?.Name);
                    logLine.Append(": ");
                    logLine.AppendLine(formatter(state, exception));
                    File.AppendAllTextAsync(fullFileName, logLine.ToString());
                    RollFiles();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RollFiles()
        {
            int maxRetainedFiles = 3;
            var files = new DirectoryInfo(_filePath)
                .GetFiles("log-" + "*")
                .OrderByDescending(f => f.Name)
                .Skip(maxRetainedFiles);

            foreach (var item in files)
            {
                item.Delete();
            }
        }
    }
}