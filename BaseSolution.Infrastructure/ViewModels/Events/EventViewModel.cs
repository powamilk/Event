using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.Events
{
    public class EventViewModel : ViewModelBase<int>
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public EventViewModel(IEventReadOnlyRepository eventReadOnlyRepository, ILocalizationService localizationService)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int eventId, CancellationToken cancellationToken)
        {
            var result = await _eventReadOnlyRepository.GetEventByIdAsync(eventId, cancellationToken);

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
