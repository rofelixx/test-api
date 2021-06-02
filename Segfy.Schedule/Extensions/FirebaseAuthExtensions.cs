using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Segfy.Schedule.Model.Configuration;

namespace Segfy.Schedule.Extensions
{
    public static class FirebaseAuthExtensions
    {
        public static IServiceCollection ResolveJWT(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var auth = serviceProvider.GetRequiredService<IOptions<AuthOptions>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = auth.Value.Authority;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = auth.Value.ValidIssuer,
                        ValidateAudience = true,
                        ValidAudience = auth.Value.ValidAudience,
                        ValidateLifetime = true,
                    };
                });

            return services;
        }

        public static void UseCookies(this IApplicationBuilder app, IConfiguration configuration)
        {
            // Cookie => Bearer middleware
            app.Use(async (context, next) =>
            {
                string jwt = context.Request.Cookies[configuration["Auth:CookieName"]];

                if (jwt != null)
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {jwt}");
                }

                await next();
            });
        }
    }

}