using Segfy.Schedule.Infra.Repositories.Base;
using Segfy.Schedule.Model.Entities;
namespace Segfy.Schedule.Infra.Repositories
{
    public interface IScheduleRepository : ITableRepository<ScheduleEntity, Model.Filters.Filter>
    {
    }
}