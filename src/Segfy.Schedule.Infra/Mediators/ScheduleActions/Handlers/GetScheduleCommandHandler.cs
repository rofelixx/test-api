using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.ViewModels;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class GetScheduleCommandHandler : IRequestHandler<GetScheduleCommand, ScheduleViewModel>
    {
        private readonly IScheduleRepository _repository;
        public GetScheduleCommandHandler(IScheduleRepository repository)
        {
            this._repository = repository;
        }

        public Task<ScheduleViewModel> Handle(GetScheduleCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Single(request.SubscriptionId, request.Id);
            // TODO mapper
            return Task.FromResult(new ScheduleViewModel());
        }
    }
}