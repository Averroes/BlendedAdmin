using BlendedAdmin.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlendedAdmin.Services
{
    public interface IUrlService
    {
        string GetUrl();
        string GetBasePath();
        string GetUrlWithQueryString(dynamic queryString);
        bool HasQueryString(string name);
        bool HasQueryString(string name, string value);
        string GetQueryString(string name);
        int GetItemId();
        string GetEnvironment();
        string GetUrlWithEnvironment(string environment);
        string GetTenant();
        string GetUrlWithTenant(string tenant);
    }

    public class UrlService : IUrlService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IActionContextAccessor _actionContextAccessor;
        private IOptions<HostingOptions> _settings;

        public UrlService(
            IHttpContextAccessor httpContextAccessor, 
            IActionContextAccessor actionContextAccessor,
            IOptions<HostingOptions> settings)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
            _settings = settings;
        }

        public string GetUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string url = request.Path + request.QueryString;
            return url;
        }

        public string GetBasePath()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.PathBase;
        }

            public string GetUrlWithQueryString(dynamic queryString)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var query = QueryHelpers.ParseQuery(request.QueryString.Value);
            var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            foreach (var prop in queryString.GetType().GetProperties())
            {
                string key = prop.Name;
                string value = prop.GetValue(queryString, null);

                items.RemoveAll(x => x.Key == key);
                if (value != null)
                    items.Add(new KeyValuePair<string, string>(key, value));
            }
            var url = request.Path  + new QueryBuilder(items).ToQueryString();
            return url;
        }

        public IList<KeyValuePair<string, string>> GetQueryString()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var query = QueryHelpers.ParseQuery(request.QueryString.Value);
            var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
            return items;
        }

        public bool HasQueryString(string name)
        {
            return GetQueryString().Any(x => x.Key == name);
        }

        public bool HasQueryString(string name, string value)
        {
            return GetQueryString().Any(x => x.Key == name && x.Value == value);
        }

        public string GetQueryString(string name)
        {
            return _httpContextAccessor.HttpContext.Request.Query[name].FirstOrDefault();
        }

        public int GetItemId()
        {
            var context = _actionContextAccessor.ActionContext;
            string controller = context.ActionDescriptor.RouteValues["controller"];
            //string action = context.ActionDescriptor.RouteValues["action"];
            if (controller == "Items")
            {
                if (context.RouteData.Values.ContainsKey("id"))
                {
                    int id = 0;
                    int.TryParse(context.RouteData.Values["id"] as string, out id);
                    return id;
                }
            }
            return 0;
        }

        public string GetEnvironment()
        {
            string environment = _actionContextAccessor.ActionContext.RouteData.Values.GetValueOrDefault("environment") as string;
            return environment;
        }

        public string GetTenant()
        {
            var request = _httpContextAccessor?.HttpContext?.Request;
            if (request != null && string.IsNullOrWhiteSpace(request.Host.Host) == false)
            {
                var subDomains = request.Host.Host.Split('.');
                return subDomains[0].Trim().ToLower();
            }
            return null;
        }

        public string GetUrlWithEnvironment(string environment)
        {
            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            var path = request.Path;
            if (path.HasValue)
            {
                var segments = path.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (segments.Any() && segments[0] == GetEnvironment())
                {
                    segments[0] = environment;
                }
                else
                {
                    segments.Insert(0, environment);
                }
                path = new PathString("/" + string.Join("/", segments));
            }
            var absoluteUri = path + request.QueryString.ToUriComponent();
            return absoluteUri; 
        }

        public string GetUrlWithTenant(string tenant)
        {
            if (string.IsNullOrEmpty(_settings?.Value?.TenantUrl) == false)
            {
                return string.Format(_settings.Value.TenantUrl, tenant);
            }
            else
            {
                return _httpContextAccessor.HttpContext.Request.Scheme + "://" + tenant + "." + _httpContextAccessor.HttpContext.Request.Host.ToUriComponent();
            }
        }
    }
}
