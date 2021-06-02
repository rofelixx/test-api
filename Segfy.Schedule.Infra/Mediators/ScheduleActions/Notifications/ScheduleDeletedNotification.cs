using System;
using MediatR;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Notifications
{
    public class ScheduleDeletedNotification : INotification
    {
        public Guid Id { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}