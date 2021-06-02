using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Segfy.Schedule.Infra.Handles;
using Segfy.Schedule.Infra.Operations;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Entities;

namespace Segfy.Schedule.Extensions
{
    public static class ConfigureRepositoryInjections
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDynamoBDFiltersHandle, DynamoBDFiltersHandle>();

            serviceCollection.AddScoped<IDynamoBDOperations<ScheduleEntity>>(sp =>
            {
                var context = sp.GetRequiredService<IDynamoDBContext>();
                var filterHandle = sp.GetRequiredService<IDynamoBDFiltersHandle>();

                return new DynamoBDOperations<ScheduleEntity>(context, filterHandle);
            });
            
            serviceCollection.AddScoped<IScheduleRepository, ScheduleRepository>();

            return serviceCollection;
        }
    }
}