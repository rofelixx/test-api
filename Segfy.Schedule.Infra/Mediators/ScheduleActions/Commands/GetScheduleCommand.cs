using System;
using MediatR;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class GetScheduleCommand : IRequest<ScheduleItemDto>
    {
        public Guid Id { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}