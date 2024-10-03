using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Event;
using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.DataTransferObjects.Organizer;
using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using BaseSolution.Application.DataTransferObjects.Participant;
using BaseSolution.Application.DataTransferObjects.Participant.Request;
using BaseSolution.Application.DataTransferObjects.Registration.Request;
using BaseSolution.Application.DataTransferObjects.Registration;
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
            CreateMap<Participant, ParticipantDto>().ReverseMap();
            CreateMap<EventCreateRequest, Event>();
            CreateMap<EventUpdateRequest, Event>();
            CreateMap<OrganizerCreateRequest, Organizer>();
            CreateMap<OrganizerUpdateRequest, Organizer>();
            CreateMap<ParticipantUpdateRequest, Participant>();
            CreateMap<Registration, RegistrationDto>();
            CreateMap<RegistrationCreateRequest, Registration>();
            CreateMap<RegistrationUpdateRequest, Registration>();
        }
    }
}
