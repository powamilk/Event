using BaseSolution.Application.DataTransferObjects.Event.Request;
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

namespace BaseSolution.Infrastructure.ViewModels.Orther
{
    public class EventSearchViewModel : ViewModelBase<EventSearchRequest>
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public EventSearchViewModel(IEventReadOnlyRepository eventReadOnlyRepository, ILocalizationService localizationService)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(EventSearchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var eventsResult = await _eventReadOnlyRepository.SearchEventsAsync(request.Name, request.Location, request.StartDate, request.EndDate, cancellationToken);

                if (eventsResult != null && eventsResult.Any())
                {
                    Data = eventsResult;  
                    Success = true;
                }
                else
                {
                    Success = false;
                    Message = _localizationService["Không tìm thấy sự kiện phù hợp."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[] { new ErrorItem { Error = ex.Message } };
                Message = _localizationService["Có lỗi xảy ra khi tìm kiếm sự kiện."];
            }
        }

    }
}
