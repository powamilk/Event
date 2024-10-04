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
    public class EventFilterViewModel : ViewModelBase<EventFilterRequest>
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public EventFilterViewModel(IEventReadOnlyRepository eventReadOnlyRepository, ILocalizationService localizationService)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(EventFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var filteredEvents = await _eventReadOnlyRepository.FilterEventsByStatusAsync(request.Status, cancellationToken);

                if (filteredEvents != null)
                {
                    Data = filteredEvents;
                    Success = true;
                }
                else
                {
                    Success = false;
                    Message = _localizationService["Không tìm thấy sự kiện với trạng thái này."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[] { new ErrorItem { Error = ex.Message } };
                Message = _localizationService["Có lỗi xảy ra khi lọc sự kiện."];
            }
        }
    }
}
