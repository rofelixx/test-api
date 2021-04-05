using System;
using MediatR;
using Segfy.Schedule.Model.ViewModels;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class GetScheduleCommand : IRequest<ScheduleViewModel>
    {
        public Guid Id { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}