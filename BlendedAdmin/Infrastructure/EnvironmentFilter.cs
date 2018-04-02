using BlendedAdmin.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlendedAdmin
{
    public class EnvironmentFilter : ActionFilterAttribute
    {
        private IServiceScopeFactory _serviceScopeFactory;
        public EnvironmentFilter(IServiceScopeFactory serviceScopeFactory)
        {
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
        }
    }
}
