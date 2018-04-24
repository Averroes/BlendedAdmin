using BlendedAdmin.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlendedJS;
using Jint.Runtime.Interop;
using BlendedAdmin.Models.Items;

namespace BlendedAdmin.Js
{
    public interface IJsService
    {
        Task<JsRunResult> Run(string code);
    }

    public class JsService : IJsService, IDisposable
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IVariablesService _variablesService;
        private IEnvironmentService _environmentService;
        private IUrlService _urlServicecs;
        private BlendedJSEngine _engine;

        public JsService(
            IHttpContextAccessor httpContextAccessor, 
            IVariablesService variablesService,
            IEnvironmentService environmentService,
            IUrlService urlServicecs)
        {
            _httpContextAccessor = httpContextAccessor;
            _variablesService = variablesService;
            _environmentService = environmentService;
            _urlServicecs = urlServicecs;
            _engine = new BlendedJSEngine();
        }

        public async Task<JsRunResult> Run(string code)
        {
            JsRunResult runResults = new JsRunResult();
            var httpContext = _httpContextAccessor.HttpContext;
            var arg = new BlendedJS.Object();
            arg["variables"] = (await this._variablesService.GetVariables()).ToJsObject();
            arg["environment"] = (await this._environmentService.GetCurrentEnvironment()).Name;
            arg["queryString"] = httpContext.Request.Query
                .ToDictionary(x => x.Key, x => (object)x.Value.FirstOrDefault())
                .ToJsObject();
            try
            {
                arg["form"] = httpContext.Request.Form
                    .ToDictionary(x => x.Key, x => (object)x.Value.FirstOrDefault())
                    .ToJsObject();
            } catch
            {
                arg["form"] = new BlendedJS.Object();
            }
            if (httpContext.Request.Query.ContainsKey("_httpMethod"))
                arg["method"] = httpContext.Request.Query["_httpMethod"].FirstOrDefault().ToLower();
            else
                arg["method"] = httpContext.Request.Method.ToLower();
            _engine.Jint.SetValue("FormView", TypeReference.CreateTypeReference(_engine.Jint, typeof(FormView)));
            _engine.Jint.SetValue("TableView", TypeReference.CreateTypeReference(_engine.Jint, typeof(TableView)));
            _engine.Jint.SetValue("JsonView", TypeReference.CreateTypeReference(_engine.Jint, typeof(JsonView)));
            _engine.Jint.SetValue("HtmlView", TypeReference.CreateTypeReference(_engine.Jint, typeof(HtmlView)));
            _engine.Jint.SetValue("arg", arg);
            var jsResult = _engine.ExecuteScript(code);
            runResults.Logs = jsResult.Logs;
            runResults.Exception = jsResult.Exception;
            runResults.LastExecutedLine = jsResult.LastExecutedLine;
            if (jsResult.Value is Array)
            {
                foreach (var view in (Array)jsResult.Value)
                {
                    if (view is TableView)
                        runResults.Views.Add(new TableViewModelAssembler(_urlServicecs).ToModel((TableView)view));
                    if (view is FormView)
                        runResults.Views.Add(new FormViewModelAssembler().ToModel((FormView)view));
                    if (view is JsonView)
                        runResults.Views.Add(new JsonViewModelAssembler().ToModel((JsonView)view));
                    if (view is HtmlView)
                        runResults.Views.Add(new HtmlViewModelAssembler().ToModel((HtmlView)view));
                }
            }
            else if (jsResult.Value is View)
            {
                if (jsResult.Value is TableView)
                    runResults.Views.Add(new TableViewModelAssembler(_urlServicecs).ToModel((TableView)jsResult.Value));
                if (jsResult.Value is FormView)
                    runResults.Views.Add(new FormViewModelAssembler().ToModel((FormView)jsResult.Value));
                if (jsResult.Value is JsonView)
                    runResults.Views.Add(new JsonViewModelAssembler().ToModel((JsonView)jsResult.Value));
                if (jsResult.Value is HtmlView)
                    runResults.Views.Add(new HtmlViewModelAssembler().ToModel((HtmlView)jsResult.Value));
            }
            return runResults;
        }

        public void Dispose()
        {
            _engine?.Dispose();
        }
    }
}
