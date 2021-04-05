using System;
using MediatR;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class CreateScheduleCommand : IRequest<ScheduleItemDto>
    {
        public Guid SubscriptionId { get; set; }
        public ScheduleCreationDto Schedule { get; set; }

    }
}