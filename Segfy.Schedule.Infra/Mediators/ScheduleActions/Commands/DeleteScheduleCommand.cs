using System;
using MediatR;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class DeleteScheduleCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}