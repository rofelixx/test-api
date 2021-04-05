using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class DeleteScheduleHandler : IRequestHandler<DeleteScheduleCommand, Unit>
    {
        private readonly IScheduleRepository _repository;
        public DeleteScheduleHandler(IScheduleRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Unit> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            await this._repository.Remove(request.SubscriptionId, request.Id);
            return Unit.Value;
        }
    }
}