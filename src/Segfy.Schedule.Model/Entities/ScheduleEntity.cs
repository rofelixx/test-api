using System;
using Amazon.DynamoDBv2.DataModel;

namespace Segfy.Schedule.Model.Entities
{
    [DynamoDBTable("schedules")]
    public class ScheduleEntity : BaseEntity
    {
        [DynamoDBProperty("type")]
        public string Type { get; set; }

        [DynamoDBProperty("description")]
        public string Description { get; set; }

        [DynamoDBProperty("date")]
        public DateTime? Date { get; set; }

        [DynamoDBProperty("recurrence")]
        public string Recurrence { get; set; }

        [DynamoDBLocalSecondaryIndexRangeKey("user_id_index")]
        [DynamoDBProperty("user_id")]
        public string UserId { get; set; }

        [DynamoDBProperty("relationship_id")]
        public string RelationshipId { get; set; }

        [DynamoDBProperty("responsible_id")]
        public string ResponsibleId { get; set; }

        [DynamoDBProperty("notes")]
        public string Notes { get; set; }
    }
}