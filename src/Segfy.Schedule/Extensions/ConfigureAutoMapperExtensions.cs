using Microsoft.Extensions.DependencyInjection;
using Segfy.Schedule.Mappers;

namespace Segfy.Schedule.Extensions
{
    public static class ConfigureAutoMapperExtensions
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(ScheduleMapper));
            return serviceCollection;
        }
    }
}