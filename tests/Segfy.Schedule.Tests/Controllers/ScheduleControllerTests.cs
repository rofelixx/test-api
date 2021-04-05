using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Segfy.Schedule.Controllers;
using Xunit;

namespace Segfy.Schedule.Tests.Controllers
{
    public class ScheduleControllerTests
    {
        [Fact]
        public async Task GetAction()
        {
            //Given
            var mockMediatr = new Mock<IMediator>();
            var controller = new ScheduleController(mockMediatr.Object);

            //When
            var item = await controller.Get(Guid.NewGuid(), new Model.Filters.FilterData());

            //Then
            item.Should().NotBeNull("response types should be consistent");
            item.IsSuccessful.Should().BeTrue("there is no reason for request to fail");
        }

        [Fact]
        public async Task GetOneAction()
        {
            //Given
            var mockMediatr = new Mock<IMediator>();
            var controller = new ScheduleController(mockMediatr.Object);

            //When
            var item = await controller.GetOne(Guid.NewGuid(), Guid.NewGuid());

            //Then
            item.Should().NotBeNull("response types should be consistent");
            item.IsSuccessful.Should().BeTrue("there is no reason for request to fail");
        }

        [Fact]
        public async Task PostAction()
        {
            //Given
            var mockMediatr = new Mock<IMediator>();
            var controller = new ScheduleController(mockMediatr.Object);

            //When
            var item = await controller.Post(Guid.NewGuid(), new Model.Dtos.ScheduleCreationDto());

            //Then
            item.Should().NotBeNull("response types should be consistent");
            item.IsSuccessful.Should().BeTrue("there is no reason for request to fail");
        }

        [Fact]
        public async Task PutAction()
        {
            //Given
            var mockMediatr = new Mock<IMediator>();
            var controller = new ScheduleController(mockMediatr.Object);

            //When
            var item = await controller.Put(Guid.NewGuid(), Guid.NewGuid(), new Model.Dtos.ScheduleCreationDto());

            //Then
            item.Should().NotBeNull("response types should be consistent");
            item.IsSuccessful.Should().BeTrue("there is no reason for request to fail");
        }

        [Fact]
        public async Task DeleteAction()
        {
            //Given
            var mockMediatr = new Mock<IMediator>();
            var controller = new ScheduleController(mockMediatr.Object);

            //When
            var item = await controller.Delete(Guid.NewGuid(), Guid.NewGuid());

            //Then
            item.Should().NotBeNull("response types should be consistent");
            item.IsSuccessful.Should().BeTrue("there is no reason for request to fail");
        }
    }
}