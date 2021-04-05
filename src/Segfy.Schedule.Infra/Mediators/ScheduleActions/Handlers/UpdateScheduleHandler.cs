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
    public class UpdateScheduleHandler : IRequestHandler<UpdateScheduleCommand, ScheduleItemDto>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateScheduleHandler(IScheduleRepository repository, IMapper mapper, IMediator mediator)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._mediator = mediator;
        }

        public async Task<ScheduleItemDto> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.Single(request.SubscriptionId, request.Id);

            var item = _mapper.Map<ScheduleEntity>(request.Schedule);
            item.Id = existing.Id;
            item.SubscriptionId = existing.SubscriptionId;
            item.CreatedAt = existing.CreatedAt;
            var entity = await _repository.Update(item);

            var response = _mapper.Map<ScheduleItemDto>(entity);
            await _mediator.Publish(CreateScheduleUpdatedNotification(response));

            return response;
        }

        private INotification CreateScheduleUpdatedNotification(ScheduleItemDto dto)
        {
            return new ScheduleUpdatedNotification()
            {
                Updated = dto,
            };
        }
    }
}