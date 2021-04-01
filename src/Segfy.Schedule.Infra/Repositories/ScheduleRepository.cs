using Segfy.Schedule.Model.Entities;
using Amazon.DynamoDBv2;
using Segfy.Schedule.Infra.Repositories.Base;

namespace Segfy.Schedule.Infra.Repositories
{
    public class ScheduleRepository : TableRepository<ScheduleEntity>, IScheduleRepository
    {
        public ScheduleRepository(IAmazonDynamoDB client) : base(client)
        {
        }
    }
}