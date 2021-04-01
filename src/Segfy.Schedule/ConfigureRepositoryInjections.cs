using Microsoft.Extensions.DependencyInjection;
using Segfy.Schedule.Infra.Repositories;

namespace Segfy.Schedule
{
    public static class ConfigureRepositoryInjections
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IScheduleRepository, ScheduleRepository>();
            return serviceCollection;
        }
    }
}