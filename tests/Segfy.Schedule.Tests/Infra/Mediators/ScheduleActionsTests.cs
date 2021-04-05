using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers;
using Segfy.Schedule.Infra.Repositories;
using Xunit;

namespace Segfy.Schedule.Tests.Infra.Mediators
{
    public class ScheduleActionsTests
    {
        [Fact]
        public async Task CreateScheduleHandler_Handle()
        {
            //Given
            var mockContext = new Mock<IScheduleRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediatr = new Mock<IMediator>();
            var command = new CreateScheduleCommand();
            var handler = new CreateScheduleHandler(mockContext.Object, mockMapper.Object, mockMediatr.Object);
            var tcs = new CancellationTokenSource(1000);

            //When
            var created = await handler.Handle(command, tcs.Token);

            //Then
            created.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteScheduleHandler_Handle()
        {
            //Given
            var mockContext = new Mock<IScheduleRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediatr = new Mock<IMediator>();
            var command = new DeleteScheduleCommand();
            var handler = new DeleteScheduleHandler(mockContext.Object, mockMediatr.Object);
            var tcs = new CancellationTokenSource(1000);

            //When
            await handler.Handle(command, tcs.Token);

            //Then
        }

        [Fact]
        public async Task GetScheduleCommandHandler_Handle()
        {
            //Given
            var mockContext = new Mock<IScheduleRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediatr = new Mock<IMediator>();
            var command = new GetScheduleCommand();
            var handler = new GetScheduleCommandHandler(mockContext.Object, mockMapper.Object);
            var tcs = new CancellationTokenSource(1000);

            //When
            var items = await handler.Handle(command, tcs.Token);

            //Then
            items.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateScheduleHandler_Handle()
        {
            //Given
            var mockContext = new Mock<IScheduleRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockMediatr = new Mock<IMediator>();
            var command = new UpdateScheduleCommand();
            var handler = new UpdateScheduleHandler(mockContext.Object, mockMapper.Object, mockMediatr.Object);
            var tcs = new CancellationTokenSource(1000);

            //When
            var created = await handler.Handle(command, tcs.Token);

            //Then
            created.Should().NotBeNull();
        }
    }
}