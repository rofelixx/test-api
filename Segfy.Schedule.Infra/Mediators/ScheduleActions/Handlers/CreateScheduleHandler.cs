using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Notifications;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Dtos;
using Segfy.Schedule.Model.Entities;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, ScheduleItemDto>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateScheduleHandler(IScheduleRepository repository, IMapper mapper, IMediator mediator)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._mediator = mediator;
        }

        public async Task<ScheduleItemDto> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<ScheduleEntity>(request.Schedule);
            item.SubscriptionId = request.SubscriptionId;
            var entity = await _repository.Add(item);

            var response = _mapper.Map<ScheduleItemDto>(entity);
            await _mediator.Publish(CreateScheduleCreatedNotification(response));

            return response;
        }

        private INotification CreateScheduleCreatedNotification(ScheduleItemDto dto)
        {
            return new ScheduleCreatedNotification()
            {
                Created = dto,
            };
        }
    }
}