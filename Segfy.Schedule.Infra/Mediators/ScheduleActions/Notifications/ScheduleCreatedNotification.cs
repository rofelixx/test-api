using MediatR;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Notifications
{
    public class ScheduleCreatedNotification : INotification
    {
        public ScheduleItemDto Created { get; set; }
    }
}