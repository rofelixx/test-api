using System;
using Amazon.DynamoDBv2.DataModel;

namespace Segfy.Schedule.Model.Entities
{
    public abstract class BaseEntity
    {
        [DynamoDBHashKey("subscription_id")]
        public Guid SubscriptionId { get; set; }

        [DynamoDBRangeKey("id")]
        public Guid Id { get; set; }
    }
}
