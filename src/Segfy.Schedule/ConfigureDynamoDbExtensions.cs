using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Segfy.Schedule.Model.Configuration;

namespace Segfy.Schedule
{
    public static class ConfigureDynamoDbExtensions
    {
        public static IServiceCollection AddDynamoDB(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<AppConfiguration>>();
                var clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = config.Value.DynamoDbUrl
                };
                return new AmazonDynamoDBClient(clientConfig);
            });

            serviceCollection.AddScoped<IDynamoDBContext>(sp =>
            {
                var client = sp.GetRequiredService<IAmazonDynamoDB>();
                return new DynamoDBContext(client);
            });

            return serviceCollection;
        }

    }
}