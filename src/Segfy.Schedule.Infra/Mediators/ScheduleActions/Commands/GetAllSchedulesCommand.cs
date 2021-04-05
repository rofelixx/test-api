using System;
using System.Collections.Generic;
using MediatR;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class GetAllSchedulesCommand : IRequest<IEnumerable<ScheduleItemDto>>
    {
        public Guid SubscriptionId { get; set; }
    }
}