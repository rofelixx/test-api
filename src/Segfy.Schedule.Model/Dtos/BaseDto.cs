using System;

namespace Segfy.Schedule.Model.Dtos
{
    public class BaseDto
    {
        public Guid SubscriptionId { get; set; }

        public Guid Id { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
