using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Infra.Repositories.Base;
using Segfy.Schedule.Infra.Operations;

namespace Segfy.Schedule.Infra.Repositories
{
    public class ScheduleRepository : TableRepository<ScheduleEntity>, IScheduleRepository
    {
        public ScheduleRepository(IDynamoBDOperations<ScheduleEntity> context) : base(context)
        {
        }
    }
}