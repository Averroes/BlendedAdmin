using BlendedAdmin.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlendedAdmin
{
    public class EnvironmentFilter : ActionFilterAttribute
    {
        IEnvironmentService _environmentService;
        public EnvironmentFilter(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        protected EnvironmentFilter()
        {
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            context.RouteData.Values["environment"] = (await _environmentService.GetCurrentEnvironment()).Name;
        }
    }
}
