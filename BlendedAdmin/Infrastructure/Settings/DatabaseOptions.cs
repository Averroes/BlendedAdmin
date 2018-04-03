using System;

namespace BlendedAdmin.Infrastructure
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }
        public string ConnectionProvider { get; set; }
        public bool MultiTenants { get; set; }
    }
}
