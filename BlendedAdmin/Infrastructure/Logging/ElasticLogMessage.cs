using Microsoft.Extensions.Logging;
using System;

namespace BlendedAdmin.Infrastructure.Logging
{
    public class ElasticLogMessage
    {
        public string LogLevel { get; set; }
        public string State { get; set; }
        public string Exception { get; set; }
        public string Message { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Category { get; set; }
    }

}