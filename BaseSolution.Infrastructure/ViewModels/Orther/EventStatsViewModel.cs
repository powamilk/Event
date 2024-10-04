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
    public class EventStatsViewModel : ViewModelBase<EventStatsRequest>
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public EventStatsViewModel(IEventReadOnlyRepository eventReadOnlyRepository, ILocalizationService localizationService)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(EventStatsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var statsResult = await _eventReadOnlyRepository.GetEventStatsAsync(request.StartDate, request.EndDate, request.Status, cancellationToken);

                if (statsResult != null)
                {
                    Data = statsResult;
                    Success = true;
                }
                else
                {
                    Success = false;
                    Message = _localizationService["Không thể lấy thống kê sự kiện."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[] { new ErrorItem { Error = ex.Message } };
                Message = _localizationService["Có lỗi xảy ra khi lấy thống kê sự kiện."];
            }
        }
    }
}
