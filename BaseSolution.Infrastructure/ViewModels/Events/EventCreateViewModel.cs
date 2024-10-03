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
        private readonly IMapper _mapper;

        public EventCreateViewModel(IEventReadWriteRepository eventReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _eventReadWriteRepository = eventReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(EventCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var eventEntity = _mapper.Map<Event>(request);
                var result = await _eventReadWriteRepository.AddEventAsync(eventEntity, cancellationToken);

                Success = result.Success;
                ErrorItems = result.Errors;
                Message = result.Message;
            }
            catch (Exception)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while creating the Event"],
                        FieldName = LocalizationString.Common.FailedToCreate + "Event"
                    }
                };
            }
        }
    }
}
