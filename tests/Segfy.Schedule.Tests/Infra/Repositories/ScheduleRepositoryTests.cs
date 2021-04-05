using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using Moq;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Entities;
using Xunit;

namespace Segfy.Schedule.Tests.Infra.Repositories
{
    public class ScheduleRepositoryTests
    {
        [Fact()]
        public async Task ScheduleRepository_Add()
        {
            //Given
            var mockContext = new Mock<IDynamoDBContext>();
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

        // [Fact()]
        // public async Task ScheduleRepository_Add_Enumerable()
        // {
        //     //Given
        //     var mockContext = new Mock<IDynamoDBContext>();
        //     mockContext.
        //     var repo = new ScheduleRepository(mockContext.Object);
        //     var entityToSave = new ScheduleEntity()
        //     {
        //         Description = "test"
        //     };
        //     var entitiesToSave = Enumerable.Range(1, 5).Select(index => new ScheduleEntity
        //     {
        //         Description = $"test {index}"
        //     });


        //     //When
        //     var savedEntities = await repo.Add(entitiesToSave);

        //     //Then
        //     savedEntities.Should().NotBeNull("saved items should be returned");
        //     savedEntities.Should().HaveCount(5, "we passed 5 items to the Add action");
        //     savedEntities.All(x => Guid.Empty != x.Id).Should().BeTrue("all items should be hydrated with new GUID");
        //     savedEntities.All(x => x.CreatedAt != null).Should().BeTrue("all items should be hydrated with new GUID");
        // }
    }
}