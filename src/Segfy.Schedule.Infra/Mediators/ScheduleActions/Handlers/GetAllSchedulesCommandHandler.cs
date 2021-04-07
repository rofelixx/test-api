using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using AutoMapper;
using MediatR;
using Segfy.Schedule.Infra.Mediators.ScheduleActions.Commands;
using Segfy.Schedule.Infra.Repositories;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Infra.Mediators.ScheduleActions.Handlers
{
    public class GetAllSchedulesCommandHandler : IRequestHandler<GetAllSchedulesCommand, PaginationDto<ScheduleItemDto>>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSchedulesCommandHandler(IScheduleRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<PaginationDto<ScheduleItemDto>> Handle(GetAllSchedulesCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Query(request.SubscriptionId, request.NextKey, request.PerPage);
            var response = _mapper.Map<IEnumerable<ScheduleItemDto>>(entity.Items);
            Guid? nextKey = null;
            if (entity.LastEvaluatedKey.TryGetValue("id", out AttributeValue value))
            {
                nextKey = Guid.Parse(value.S);
            }

            return new PaginationDto<ScheduleItemDto>()
            {
                Items = response,
                NextKey = nextKey,
            };
        }
    }
}