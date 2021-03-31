using Segfy.Schedule.Infra.Repositories;
using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Segfy.Schedule.Tests.Integration.DynamoDB.Model;

namespace Segfy.Schedule.Tests.Integration.DynamoDB
{
    public class DummyTableRepository : TableRepository<DummyTable, DummyTableViewModel>
    {
        public DummyTableRepository(IAmazonDynamoDB client) : base(client)
        {
        }

        protected override async Task<DummyTable> HydratateEntityForUpdate(Guid hashid, Guid sortid, DummyTableViewModel entity)
        {
            var item = await base.Single(hashid, sortid);
            item.DummyIndex = entity.DummyIndex;
            item.DummyInteger = entity.DummyInteger;
            item.Text = entity.Text;
            return item;
        }

        protected override Task<DummyTable> HydrateEntityForCreation(DummyTableViewModel entity)
        {
            return Task.FromResult(new DummyTable()
            {
                Id = Guid.NewGuid(),
                SubscriptionId = entity.SubscriptionId,
                DummyIndex = entity.DummyIndex,
                DummyInteger = entity.DummyInteger,
                Text = entity.Text,
            });
        }
    }
}