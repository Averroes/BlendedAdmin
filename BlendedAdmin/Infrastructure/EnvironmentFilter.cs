using BlendedAdmin.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlendedAdmin
{
    public class EnvironmentFilter : ActionFilterAttribute
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private IEnvironmentService _environmentService;
        public EnvironmentFilter(
            //IEnvironmentService environmentService, 
            IServiceScopeFactory serviceScopeFactory)
        {
            //_environmentService = environmentService;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        protected EnvironmentFilter()
        {
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var environmentService = scope.ServiceProvider.GetRequiredService<IEnvironmentService>();
                context.RouteData.Values["environment"] = (await environmentService.GetCurrentEnvironment()).Name;
            }

            //context.RouteData.Values["environment"] = (await _environmentService.GetCurrentEnvironment()).Name;
        }
    }
}
