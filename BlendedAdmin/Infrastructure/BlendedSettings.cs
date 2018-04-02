using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Infrastructure
{
    public class BlendedSettings
    {
        public bool MultiTenants { get; set; }
    }

    public class MailSettings
    {
        public string SmtpFrom { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
