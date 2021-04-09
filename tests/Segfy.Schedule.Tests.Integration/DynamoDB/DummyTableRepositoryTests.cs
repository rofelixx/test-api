using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using System;
using Segfy.Schedule.Tests.Integration.DynamoDB.Model;
using Segfy.Schedule.Model.Filters;

namespace Segfy.Schedule.Tests.Integration.DynamoDB
{
    public class DummyTableRepositoryTests : IClassFixture<AmazonDynamoDbFixture>
    {
        private readonly AmazonDynamoDbFixture fixture;

        public DummyTableRepositoryTests(AmazonDynamoDbFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task DummyTableRepository_All()
        {
            var repo = new DummyTableRepository(fixture.DbOperations);
            var items = await repo.All(25);

            items.Should().NotBeNull();
            items.Items.Count().Should().Be(25);
            var first = items.Items.FirstOrDefault();

            items = await repo.All(25, items.PaginationToken);

            items.Should().NotBeNull();
            items.Items.Count().Should().Be(25);
            var second = items.Items.FirstOrDefault();

            first.Id.Should().NotBe(second.Id);
        }

        [Fact]
        public async Task DummyTableRepository_Find()
        {
            var repo = new DummyTableRepository(fixture.DbOperations);
            var filter = new Filter()
            {
                Field = "dummy_index",
                Value = new string[] { "5753a917-18cb-4ccc-ac07-325e5a5da259" }
            };
            var items = await repo.Find("dummy_index", filter);
            var totalItems = new List<DummyTable>();
            totalItems.AddRange(items.Items);

            while (!items.IsDone)
            {
                items = await repo.Find("dummy_index", filter, 10, items.PaginationToken);
                totalItems.AddRange(items.Items);
            }

            totalItems.GroupBy(x => x.Id).Count().Should().Be(50);
            totalItems.Count().Should().Be(50);
        }

        [Fact]
        public async Task DummyTableRepository_Find_All_Index()
        {
            var repo = new DummyTableRepository(fixture.DbOperations);
            var filter = new Filter()
            {
                Field = "dummy_index",
                Value = new string[] { "5753a917-18cb-4ccc-ac07-325e5a5da259" }
            };

            var totalItems = new List<DummyTable>();
            var items = await repo.Find("dummy_index", filter);
            totalItems.AddRange(items.Items);

            while (!items.IsDone)
            {
                items = await repo.Find("dummy_index", filter, 10, items.PaginationToken);
                totalItems.AddRange(items.Items);
            }

            totalItems.GroupBy(x => x.Id).Count().Should().Be(50);
            totalItems.Count().Should().Be(50);
        }

        [Fact]
        public async Task DummyTableRepository_Single()
        {
            var repo = new DummyTableRepository(fixture.DbOperations);

            var items = await repo.Single(fixture.EntityForSingle.SubscriptionId, fixture.EntityForSingle.Id);
            items.Should().NotBeNull();
            items.Text.Should().Be("teste");
            items.DummyInteger.Should().Be(1);
            items.DummyIndex.Should().Be("5753a917-18cb-4ccc-ac07-325e5a5da259");
        }

        [Fact]
        public async Task DummyTableRepository_Update()
        {
            var repo = new DummyTableRepository(fixture.DbOperations);
            var randomNum = new Random().Next();

            var a = new DummyTable()
            {
                Id = fixture.EntityForUpdate.Id,
                SubscriptionId = fixture.EntityForUpdate.SubscriptionId,
                DummyIndex = "5753a917-18cb-4ccc-ac07-325e5a5da259",
                DummyInteger = 3,
                Text = $"teste atualizado {randomNum}"
            };

            await repo.Update(a);

            var items = await repo.Single(fixture.EntityForUpdate.SubscriptionId, fixture.EntityForUpdate.Id);

            items.Text.Should().Be($"teste atualizado {randomNum}");
        }

        [Fact]
        public async Task DummyTableRepository_QueryPagination()
        {
            var s = Guid.Parse("c89d0a05-1022-46da-b68e-8db86df5fb3c");
            var repo = new DummyTableRepository(fixture.DbOperations);
            var q = await repo.Query(s);
            var allItems = new List<DummyTable>();
            allItems.AddRange(q.Items);
            while (q.LastEvaluatedKey.Any())
            {
                var subsid = q.LastEvaluatedKey["subscription_id"].S;
                var id = q.LastEvaluatedKey["id"].S;

                q = await repo.Query(Guid.Parse(subsid), Guid.Parse(id));
                allItems.AddRange(q.Items);
            }

            allItems.Should().HaveCount(100);
            allItems.GroupBy(x => x.Id).Count().Should().Be(100);
        }
    }
}
