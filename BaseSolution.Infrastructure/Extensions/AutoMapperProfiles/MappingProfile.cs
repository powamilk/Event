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
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.DataTransferObjects.Review;

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
            CreateMap<ReviewCreateRequest, Review>().ReverseMap();
            CreateMap<ReviewUpdateRequest, Review>().ReverseMap();
            CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
            .ForMember(dest => dest.ParticipantName, opt => opt.MapFrom(src => src.Participant.Name));
        }
    }
}
