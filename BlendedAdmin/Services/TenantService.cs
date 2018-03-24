using BlendedAdmin.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlendedAdmin.Services
{
    public interface ITenantService
    {
        string GetCurrentTenantId();
    }

    public class TenantService : ITenantService
    {
        private IUrlServicecs _urlService;

        public TenantService(IUrlServicecs urlService)
        {
            this._urlService = urlService;
        }

        public string GetCurrentTenantId()
        {
            return null;
        }
    }
}
