using Segfy.Schedule.Model.Enuns;
using System;
using System.Text.Json.Serialization;

namespace Segfy.Schedule.Model.Dtos
{
    public class ScheduleItemDto : BaseDto
    {
        public ScheduleTypes? Type { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public Recurrence? Recurrence { get; set; }

        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("relationship_id")]
        public Guid RelationshipId { get; set; }

        [JsonPropertyName("responsible_id")]
        public Guid ResponsibleId { get; set; }

        public string Notes { get; set; }
    }
}
