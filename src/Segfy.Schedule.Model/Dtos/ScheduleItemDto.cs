using Segfy.Schedule.Model.Enuns;
using System;

namespace Segfy.Schedule.Model.Dtos
{
    public class ScheduleItemDto : BaseDto
    {
        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public Recurrence? Recurrence { get; set; }

        public Guid UserId { get; set; }

        public Guid RelationshipId { get; set; }

        public Guid ResponsibleId { get; set; }

        public string Notes { get; set; }
    }
}
