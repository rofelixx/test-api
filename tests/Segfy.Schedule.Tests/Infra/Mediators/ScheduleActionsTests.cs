using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Dtos;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Pagination;
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
            mockMapper
                .Setup(x => x.Map<ScheduleEntity>(It.IsAny<ScheduleItemDto>()))
                .Returns(new ScheduleEntity());
            mockMapper
                .Setup(x => x.Map<ScheduleItemDto>(It.IsAny<ScheduleEntity>()))
                .Returns(new ScheduleItemDto());
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
            mockContext
               .Setup(x => x.Single(It.IsAny<Guid>(), It.IsAny<Guid>()))
               .ReturnsAsync(new ScheduleEntity());

            var mockMapper = new Mock<IMapper>();
            mockMapper
               .Setup(x => x.Map<ScheduleItemDto>(It.IsAny<ScheduleEntity>()))
               .Returns(new ScheduleItemDto());
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
        public async Task GetAllSchedulesCommandHandler_Handle()
        {
            //Given
            var mockContext = new Mock<IScheduleRepository>();
            mockContext
               .Setup(x => x.Query(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), null))
               .ReturnsAsync(new DynamoDBPagedRequest<ScheduleEntity>() { LastEvaluatedKey = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>() });

            var mockMapper = new Mock<IMapper>();
            mockMapper
               .Setup(x => x.Map<IEnumerable<ScheduleItemDto>>(It.IsAny<IEnumerable<ScheduleEntity>>()))
               .Returns(new List<ScheduleItemDto>());
            var mockMediatr = new Mock<IMediator>();
            var command = new GetAllSchedulesCommand();
            var handler = new GetAllSchedulesCommandHandler(mockContext.Object, mockMapper.Object);
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
            mockContext
              .Setup(x => x.Single(It.IsAny<Guid>(), It.IsAny<Guid>()))
              .ReturnsAsync(new ScheduleEntity());

            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(x => x.Map<ScheduleEntity>(It.IsAny<ScheduleItemDto>()))
                .Returns(new ScheduleEntity());
            mockMapper
               .Setup(x => x.Map<ScheduleItemDto>(It.IsAny<ScheduleEntity>()))
               .Returns(new ScheduleItemDto());

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