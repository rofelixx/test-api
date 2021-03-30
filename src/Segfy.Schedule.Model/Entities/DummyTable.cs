using Amazon.DynamoDBv2.DataModel;

namespace Segfy.Schedule.Model.Entities
{
    [DynamoDBTable("dummy-table")]
    public class DummyTable : BaseEntity
    {
        [DynamoDBGlobalSecondaryIndexHashKey("dummy_index")]
        [DynamoDBProperty("dummy_index")]
        public string DummyIndex { get; set; }

        [DynamoDBProperty("dummy_integer")]
        public int DummyInteger { get; set; }

        [DynamoDBProperty("text")]
        public string Text { get; set; }

    }
}