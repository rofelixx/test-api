using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class GetScheduleCommandHandler : IRequestHandler<GetScheduleCommand, ScheduleItemDto>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMapper _mapper;

        public GetScheduleCommandHandler(IScheduleRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<ScheduleItemDto> Handle(GetScheduleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Single(request.SubscriptionId, request.Id);
            var response = _mapper.Map<ScheduleItemDto>(entity);

            return response;
        }
    }
}