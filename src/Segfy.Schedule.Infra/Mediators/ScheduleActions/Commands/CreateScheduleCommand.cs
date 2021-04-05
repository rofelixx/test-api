using System;
using MediatR;
using Segfy.Schedule.Model.ViewModels;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class CreateScheduleCommand : IRequest<ScheduleViewModel>
    {
        public Guid SubscriptionId { get; set; }
        public ScheduleViewModel Schedule { get; set; }

    }
}