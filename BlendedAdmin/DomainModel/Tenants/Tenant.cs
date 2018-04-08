using System;
using System.Collections.Generic;

namespace BlendedAdmin.DomainModel.Tenants
{
    public class Tenant
    {
        public Tenant()
        {   
        }
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
