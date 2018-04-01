using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlendedAdmin.Infrastructure.Logging
{
    public static class ElasticLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddElastic(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, ElasticLoggerProvider>();
            return builder;
        }

        public static ILoggingBuilder AddBlended(this ILoggingBuilder builder, Action<ElasticLoggerOptions> configure)
        {
            builder.AddElastic();
            builder.Services.Configure(configure);

            return builder;
        }
    }

}