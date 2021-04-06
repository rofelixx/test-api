using AutoWrapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Segfy.Schedule.Extensions;
using Segfy.Schedule.Filters;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Configuration;

namespace Segfy.Schedule
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddConsulConfiguration(configuration);

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));
            services.Configure<AuthOptions>(Configuration.GetSection("Auth"));

            services.AddControllers(config =>
            {
                config.Filters.Add<ValidationFilter>();
                config.Filters.Add<AuthenticationFilter>();
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
            services.AddDynamoDB();
            services.AddRepositories();
            services.AddMediatR(typeof(IScheduleRepository));
            services.AddCustomLocalization();
            services.AddCustomAutoMapper();
            services.AddHttpContextAccessor();

            services.ResolveJWT();

            services.AddSwaggerConfig();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Segfy.Schedule v1"));
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { UseCustomSchema = true, ShowStatusCode = true });
            app.UseRouting();

            app.UseCookies(Configuration);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
