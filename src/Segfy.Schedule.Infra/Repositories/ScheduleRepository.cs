using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Infra.Repositories.Base;
using Amazon.DynamoDBv2.DataModel;

namespace Segfy.Schedule.Infra.Repositories
{
    public class ScheduleRepository : TableRepository<ScheduleEntity>, IScheduleRepository
    {
        public ScheduleRepository(IDynamoDBContext context) : base(context)
        {
        }
    }
}