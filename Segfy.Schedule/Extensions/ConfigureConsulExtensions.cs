using System;
using Microsoft.Extensions.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace Segfy.Schedule.Extensions
{
    public static class ConfigureConsulExtensions
    {
        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, IConfiguration configuration)
        {
            configurationBuilder.AddConsul(
                    $"segfy_mais/schedule/api/appsettings.json",
                    options =>
                    {
                        options.ConsulConfigurationOptions = cco =>
                        {
                            cco.Address = new Uri(configuration["Consul:Url"]);
                            cco.Token = configuration["Consul:Token"];
                        };
                        options.Optional = true;
                        options.ReloadOnChange = true;
                        options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                    })
                .AddEnvironmentVariables();

            return configurationBuilder;
        }

    }
}