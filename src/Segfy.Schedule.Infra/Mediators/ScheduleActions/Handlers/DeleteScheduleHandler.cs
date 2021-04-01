using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class DeleteScheduleHandler : AsyncRequestHandler<DeleteScheduleCommand>
    {
        private readonly IScheduleRepository _repository;
        public DeleteScheduleHandler(IScheduleRepository repository)
        {
            this._repository = repository;
        }

        protected override Task Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            return this._repository.Remove(request.SubscriptionId, request.Id);
        }
    }
}