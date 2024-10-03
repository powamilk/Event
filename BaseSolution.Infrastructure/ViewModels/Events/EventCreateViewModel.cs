using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.Events
{
    public class EventCreateViewModel : ViewModelBase<EventCreateRequest>
    {
        private readonly IEventReadWriteRepository _eventReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public EventCreateViewModel(IEventReadWriteRepository eventReadWriteRepository, ILocalizationService localizationService)
        {
            _eventReadWriteRepository = eventReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(EventCreateRequest request, CancellationToken cancellationToken)
        {
            var eventEntity = new Event
            {
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                MaxParticipants = request.MaxParticipants
            };

            var result = await _eventReadWriteRepository.AddEventAsync(eventEntity, cancellationToken);

            if (result.Success)
            {
                Data = result.Data; 
                Success = true;
            }
            else
            {
                Success = false;
                ErrorItems = result.Errors;
                Message = result.Message;
            }
        }
    }
}
