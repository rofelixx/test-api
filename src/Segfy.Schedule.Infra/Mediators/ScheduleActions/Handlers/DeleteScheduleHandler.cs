using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Notifications;
using Segfy.Schedule.Infra.Repositories;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class DeleteScheduleHandler : IRequestHandler<DeleteScheduleCommand, Unit>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMediator _mediator;

        public DeleteScheduleHandler(IScheduleRepository repository, IMediator mediator)
        {
            this._repository = repository;
            this._mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            await this._repository.Remove(request.SubscriptionId, request.Id);
            await _mediator.Publish(CreateScheduleDeletedNotification(request.SubscriptionId, request.Id));
            return Unit.Value;
        }

        private INotification CreateScheduleDeletedNotification(Guid subscriptionId, Guid id)
        {
            return new ScheduleDeletedNotification()
            {
                Id = id,
                SubscriptionId = id,
            };
        }
    }
}