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
                    x => x.Type,
                    map => map.MapFrom(src =>  src.Type))
                .ForMember(
                    x => x.Recurrence,
                    map => map.MapFrom(src => src.Recurrence));

            CreateMap<ScheduleCreationDto, ScheduleEntity>()
                .ForMember(
                    x => x.Type,
                    map => map.MapFrom(src => src.Type.ToLower()))
                .ForMember(
                    x => x.Recurrence,
                    map => map.MapFrom(src => src.Recurrence.ToLower()));
        }
    }
}