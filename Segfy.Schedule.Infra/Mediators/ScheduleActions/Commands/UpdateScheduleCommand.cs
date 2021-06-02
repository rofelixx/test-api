using System;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands
{
    public class UpdateScheduleCommand : CreateScheduleCommand
    {
        public Guid Id { get; set; }
    }
}