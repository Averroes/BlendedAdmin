using System;

namespace BlendedAdmin.Infrastructure
{
    public class HostingOptions
    {
        public bool MultiTenants { get; set; }
        public string TenantUrl { get; set; }
    }
}
