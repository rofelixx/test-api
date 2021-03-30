using System.Threading.Tasks;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.ViewModels;
using Xunit;
using Amazon.DynamoDBv2;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using Segfy.Schedule.Model.Entities;
using System;

namespace Segfy.Schedule.Tests.Integration.Infra
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
            var repo = new DummyTableRepository(fixture.DbClient);
            var items = await repo.All();

            items.Should().NotBeNull();
            items.Items.Count().Should().Be(25);
            var first = items.Items.FirstOrDefault();

            items = await repo.All(items.PaginationToken);

            items.Should().NotBeNull();
            items.Items.Count().Should().Be(25);
            var second = items.Items.FirstOrDefault();

            first.Id.Should().NotBe(second.Id);
        }

        [Fact]
        public async Task DummyTableRepository_Find()
        {
            var repo = new DummyTableRepository(fixture.DbClient);
            var filter = new Model.Filters.Filter()
            {
                Field = "dummy_index",
                Value = "5753a917-18cb-4ccc-ac07-325e5a5da259"
            };
            var items = await repo.Find(filter);

            items.Should().NotBeNull();
            var first = items.Items.FirstOrDefault();

            items = await repo.Find(filter, items.PaginationToken);

            items.Should().NotBeNull();
            var second = items.Items.FirstOrDefault();

            first.Id.Should().NotBe(second.Id);
        }

        [Fact]
        public async Task DummyTableRepository_Find_All()
        {
            var repo = new DummyTableRepository(fixture.DbClient);
            var filter = new Model.Filters.Filter()
            {
                Field = "dummy_index",
                Value = "5753a917-18cb-4ccc-ac07-325e5a5da259"
            };

            var totalItems = new List<DummyTable>();
            var items = await repo.Find(filter);
            do
            {
                totalItems.AddRange(items.Items);
                if (totalItems.Count > 10) break;
                items = await repo.Find(filter, items.PaginationToken);
            } while (!items.IsDone);

            totalItems.Count().Should().Be(11);
        }

        [Fact]
        public async Task DummyTableRepository_Single()
        {
            var repo = new DummyTableRepository(fixture.DbClient);

            var items = await repo.Single(fixture.EntityForSingle.SubscriptionId, fixture.EntityForSingle.Id);
            items.Should().NotBeNull();
            items.Text.Should().Be("teste");
            items.DummyInteger.Should().Be(1);
            items.DummyIndex.Should().Be("5753a917-18cb-4ccc-ac07-325e5a5da259");
        }

        [Fact]
        public async Task DummyTableRepository_Update()
        {
            var repo = new DummyTableRepository(fixture.DbClient);
            var randomNum = new Random().Next();

            var a = new DummyTableViewModel()
            {
                DummyIndex = "15fe1829-0930-45b4-adc2-28d6ad8142d8",
                DummyInteger = 3,
                Text = $"teste atualizado {randomNum}"
            };

            await repo.Update(fixture.EntityForUpdate.SubscriptionId, fixture.EntityForUpdate.Id, a);

            var items = await repo.Single(fixture.EntityForUpdate.SubscriptionId, fixture.EntityForUpdate.Id);

            items.Text.Should().Be($"teste atualizado {randomNum}");
        }
    }
}
