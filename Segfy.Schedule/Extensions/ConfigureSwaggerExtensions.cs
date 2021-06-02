using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Segfy.Schedule.Util;

namespace Segfy.Schedule.Extensions
{
    public static class ConfigureSwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Agendamento",
                    Description = "API global de tratamento de agendamentos",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<XmlCommentsEscapeFilter>();
            });

            return serviceCollection;
        }
    }
}