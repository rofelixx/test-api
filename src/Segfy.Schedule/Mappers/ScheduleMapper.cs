using System;
using AutoMapper;
using Segfy.Schedule.Model.Dtos;
using Segfy.Schedule.Model.Entities;
using Segfy.Schedule.Model.Enuns;

namespace Segfy.Schedule.Mappers
{
    public class ScheduleMapper : Profile
    {
        public ScheduleMapper()
        {
            CreateMap<ScheduleEntity, ScheduleItemDto>()
                .ForMember(
                    x => x.Recurrence,
                    map => map.MapFrom(src => Enum.Parse(typeof(Recurrence), src.Recurrence, true)));

            CreateMap<ScheduleCreationDto, ScheduleEntity>()
                .ForMember(
                    x => x.Recurrence,
                    map => map.MapFrom(src => src.Recurrence.GetValueOrDefault().ToString().ToLower()));
        }
    }
}