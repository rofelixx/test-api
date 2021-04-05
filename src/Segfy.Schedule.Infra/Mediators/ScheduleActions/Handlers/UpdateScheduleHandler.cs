using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.ViewModels;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class UpdateScheduleHandler : IRequestHandler<UpdateScheduleCommand, ScheduleViewModel>
    {
        private readonly IScheduleRepository _repository;
        public UpdateScheduleHandler(IScheduleRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ScheduleViewModel> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.Single(request.SubscriptionId, request.Id);
            // TODO mapper
            var entity = _repository.Update(item);
            // TODO publish para log
            // TODO mapper
            return new ScheduleViewModel();
        }

    }
}