using System;
using System.Text.Json.Serialization;

namespace Segfy.Schedule.Model.Dtos
{
    public class BaseDto
    {
        [JsonPropertyName("subscription_id")]
        public Guid SubscriptionId { get; set; }

        public Guid Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
