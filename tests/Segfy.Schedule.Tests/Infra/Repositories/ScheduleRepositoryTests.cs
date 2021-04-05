using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using Moq;
using Segfy.Schedule.Infra.Operations;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;
using Xunit;

namespace Segfy.Schedule.Tests.Infra.Repositories
{
    public class ScheduleRepositoryTests
    {
        [Fact()]
        public async Task ScheduleRepository_Add()
        {
            //Given
            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            var repo = new ScheduleRepository(mockContext.Object);
            var entityToSave = new ScheduleEntity()
            {
                Description = "test"
            };

            //When
            var savedEntity = await repo.Add(entityToSave);

            //Then
            savedEntity.Should().NotBeNull("saved item should be returned");
            savedEntity.Id.Should().NotBeEmpty("hydratation should generate new items GUID");
            savedEntity.CreatedAt.Should().NotBeNull("hydratation should fill current UTC date/time");
            savedEntity.Description.Should().Be("test", "this is the text we passed forward");
        }

        [Fact()]
        public async Task ScheduleRepository_Add_Enumerable()
        {
            //Given
            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            var repo = new ScheduleRepository(mockContext.Object);
            var entityToSave = new ScheduleEntity()
            {
                Description = "test"
            };
            var entitiesToSave = Enumerable.Range(1, 5).Select(index => new ScheduleEntity
            {
                Description = $"test {index}"
            });


            //When
            var savedEntities = await repo.Add(entitiesToSave);

            //Then
            savedEntities.Should().NotBeNull("saved items should be returned");
            savedEntities.Should().HaveCount(5, "we passed 5 items to the Add action");
            savedEntities.All(x => Guid.Empty != x.Id).Should().BeTrue("all items should be hydrated with new GUID");
            savedEntities.All(x => x.CreatedAt != null).Should().BeTrue("all items should be hydrated with new GUID");
        }


        [Fact()]
        public async Task ScheduleRepository_All()
        {
            //Given
            var items = Enumerable.Range(1, 5).Select(index => new ScheduleEntity
            {
                Description = $"test {index}"
            });

            var pagedRequest = new DynamoDBPagedRequest<ScheduleEntity>()
            {
                Items = items,
            };

            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            mockContext.Setup(x => x.ScanAsync(It.IsAny<ScanParameters>())).Returns(Task.FromResult(pagedRequest));
            var repo = new ScheduleRepository(mockContext.Object);

            //When
            var savedEntities = await repo.All();

            //Then
            savedEntities.Should().NotBeNull("setup items should be returned");
            savedEntities.Items.Should().HaveCount(5, "we passed 5 items to the Setup");
        }

        [Fact()]
        public async Task ScheduleRepository_Find()
        {
            //Given
            var items = Enumerable.Range(1, 5).Select(index => new ScheduleEntity
            {
                Description = $"test {index}"
            });

            var entitiesToSave = new DynamoDBPagedRequest<ScheduleEntity>()
            {
                Items = items,
            };

            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            mockContext.Setup(x => x.ScanAsync(It.IsAny<ScanParameters>())).Returns(Task.FromResult(entitiesToSave));
            var repo = new ScheduleRepository(mockContext.Object);

            //When
            var savedEntities = await repo.Find(new Model.Filters.Filter() { });

            //Then
            savedEntities.Should().NotBeNull("setup items should be returned");
            savedEntities.Items.Should().HaveCount(5, "we passed 5 items to the Setup");
        }

        [Fact()]
        public async Task ScheduleRepository_Remove()
        {
            //Given
            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            var repo = new ScheduleRepository(mockContext.Object);

            //When
            await repo.Remove(Guid.Empty, Guid.Empty);

            //Then
        }

        [Fact()]
        public async Task ScheduleRepository_Single()
        {
            //Given
            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            mockContext.Setup(x => x.LoadAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new ScheduleEntity
            {
                Description = $"test"
            }));
            var repo = new ScheduleRepository(mockContext.Object);

            //When
            var item = await repo.Single(Guid.Empty, Guid.Empty);

            //Then
            item.Should().NotBeNull("single item should be returned");
            item.Description.Should().Be("test", "single should return object 'as-is' from db");
        }

        [Fact()]
        public async Task ScheduleRepository_Update()
        {
            //Given
            var mockContext = new Mock<IDynamoBDOperations<ScheduleEntity>>();
            var repo = new ScheduleRepository(mockContext.Object);
            var itemId = Guid.NewGuid();
            var subcriptionId = Guid.NewGuid();
            var entityToSave = new ScheduleEntity()
            {
                Id = itemId,
                SubscriptionId = subcriptionId,
                Description = "test"
            };

            //When
            var item = await repo.Update(entityToSave);

            //Then
            item.Should().NotBeNull("updated item should be returned");
            item.Id.Should().Be(itemId, "update should not modify original Id");
            item.SubscriptionId.Should().Be(subcriptionId, "update should not modify original SubscriptionId");
            item.Description.Should().Be("test", "update should return object 'as-is' from db");
        }
    }
}