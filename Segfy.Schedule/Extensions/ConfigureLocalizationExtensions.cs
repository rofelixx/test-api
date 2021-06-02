using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Segfy.Schedule.Extensions
{
    public static class ConfigureLocalizationExtensions
    {
        public static IServiceCollection AddCustomLocalization(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLocalization(options => options.ResourcesPath = "Resources");
            serviceCollection.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("pt-BR")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider() };
                });

            return serviceCollection;
        }
    }
}