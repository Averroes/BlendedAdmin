using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedAdmin.Infrastructure
{
    public class RedirectToProxiedHttpsRule : IRule
    {
        public virtual void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            string reqProtocol;
            if (request.Headers.ContainsKey("X-Forwarded-Proto"))
            {
                reqProtocol = request.Headers["X-Forwarded-Proto"][0];
            }
            else
            {
                reqProtocol = (request.IsHttps ? "https" : "http");
            }

            if (reqProtocol != "https")
            {
                var newUrl = new StringBuilder()
                    .Append("https://").Append(request.Host)
                    .Append(request.PathBase).Append(request.Path)
                    .Append(request.QueryString);

                context.HttpContext.Response.Redirect(newUrl.ToString(), true);
            }
        }
    }

    public static class RewriteOptionsExtensions
    {
        public static RewriteOptions AddRedirectToProxiedHttps(this RewriteOptions options)
        {
            options.Rules.Add(new RedirectToProxiedHttpsRule());
            return options;
        }
    }
}
