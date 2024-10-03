using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.Events
{
    public class EventDeleteViewModel : ViewModelBase<EventDeleteRequest>
    {
        private readonly IEventReadWriteRepository _eventReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public EventDeleteViewModel(IEventReadWriteRepository eventReadWriteRepository, ILocalizationService localizationService)
        {
            _eventReadWriteRepository = eventReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(EventDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _eventReadWriteRepository.DeleteEventAsync(request.Id, cancellationToken);

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
                        Error = _localizationService["Error occurred while deleting the Event"],
                        FieldName = LocalizationString.Common.FailedToDelete + "Event"
                    }
                };
            }
        }
    }
}
