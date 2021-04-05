using MediatR;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Notifications
{
    public class ScheduleUpdatedNotification : INotification
    {
        public ScheduleItemDto Updated { get; set; }
    }
}