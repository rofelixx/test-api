using System;

namespace Segfy.Schedule.Tests.Integration.DynamoDB.Model
{
    public class DummyTableViewModel
    {
        public Guid SubscriptionId { get; set; }
        public string DummyIndex { get; set; }
        public int DummyInteger { get; set; }
        public string Text { get; set; }
    }
}