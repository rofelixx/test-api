using Segfy.Schedule.Model.Enuns;
using System;

namespace Segfy.Schedule.Model.Dtos
{
    public class ScheduleItemDto : BaseDto
    {
        public string Kind { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public Recurrence? Recurrence { get; set; }

        public Guid InsuredId { get; set; }

        public Guid AccountableId { get; set; }

        public string Observations { get; set; }
    }
}
