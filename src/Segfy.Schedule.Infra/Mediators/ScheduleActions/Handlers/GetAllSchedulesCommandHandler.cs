using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class GetAllSchedulesCommandHandler : IRequestHandler<GetAllSchedulesCommand, IEnumerable<ScheduleItemDto>>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSchedulesCommandHandler(IScheduleRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleItemDto>> Handle(GetAllSchedulesCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.SubscriptionId);
            var response = _mapper.Map<IEnumerable<ScheduleItemDto>>(entity.Items);

            return response;
        }
    }
}