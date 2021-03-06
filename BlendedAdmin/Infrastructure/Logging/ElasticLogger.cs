﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Options;
using BlendedAdmin.Services;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace BlendedAdmin.Infrastructure.Logging
{
    public class ElasticLogger : ILogger
    {
        private string _category;
        private IOptions<ElasticLoggerOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        private IServiceScopeFactory _serviceScopeFactory;
        private IUrlService _urlService;

        private LogLevel _logLevel;

        public ElasticLogger(
            string category,
            IOptions<ElasticLoggerOptions> options, 
            IHttpContextAccessor httpContextAccessor,
            IServiceScopeFactory serviceScopeFactory,
            IUrlService urlService)
        {
            _category = category;
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
            _urlService = urlService;
            _logLevel = options.Value.LogLevel ?? LogLevel.None;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            bool isEnabled = logLevel >= _logLevel;
            return isEnabled;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                if (IsEnabled(logLevel))
                {
                    var message = new ElasticLogMessage
                    {
                        LogLevel = logLevel.ToString(),
                        State = state.ToString(),
                        Message = formatter(state, exception),
                        Exception = exception?.ToString(),
                        DateTime = DateTime.Now,
                        Category = _category,
                        UserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name,
                        TenantId = _urlService.GetTenant(),
                        ActivityId = Activity.Current?.Id?.ToString(),
                        TraceIdentifier = _httpContextAccessor.HttpContext?.TraceIdentifier?.ToString()
                    };
                    string url = _options.Value.Url;
                    HttpClient httpClient = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
                    AddAuthenticationHeader(httpClient, url);
                    httpClient.PostAsync(url,content).ContinueWith(x =>
                    {
                        if (x.Result.IsSuccessStatusCode == false)
                            Console.WriteLine("ElasticLogger error " + x.Result.StatusCode + " " + x.Result.ReasonPhrase);
                    });
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ElasticLogger error " + ex.Message);
            }
        }

        private void AddAuthenticationHeader(HttpClient client, string url)
        {
            Uri parsedUrl = new Uri(url);
            if (string.IsNullOrEmpty(parsedUrl.UserInfo) == false)
            {
                var byteArray = Encoding.ASCII.GetBytes(parsedUrl.UserInfo);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
        }
    }

}