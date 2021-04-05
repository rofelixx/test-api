using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.ViewModels;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, ScheduleViewModel>
    {
        private readonly IScheduleRepository _repository;
        public CreateScheduleHandler(IScheduleRepository repository)
        {
            this._repository = repository;
        }

        public Task<ScheduleViewModel> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            // TODO mapper
            var item = new ScheduleEntity();
            item.SubscriptionId = request.SubscriptionId;
            var entity = _repository.Add(item);
            // TODO criar Publish para que seja criada a notificação
            // TODO mapper
            return Task.FromResult(new ScheduleViewModel());
        }

    }
}