using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Event;
using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.DataTransferObjects.Organizer;
using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.AutoMapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<Organizer, OrganizerDto>().ReverseMap();
            CreateMap<EventCreateRequest, Event>();
            CreateMap<EventUpdateRequest, Event>();
            CreateMap<OrganizerCreateRequest, Organizer>();
            CreateMap<OrganizerUpdateRequest, Organizer>();
        }
    }
}
