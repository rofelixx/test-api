using Segfy.Schedule.Infra.Repositories.Base;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.ViewModels;

namespace Segfy.Schedule.Infra.Repositories
{
    public interface IScheduleRepository : ITableRepository<ScheduleEntity, Model.Filters.Filter>
    {
    }
}