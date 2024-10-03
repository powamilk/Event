using BaseSolution.Application.DataTransferObjects.Example.Request;
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
    public class EventListWithPaginationViewModel : ViewModelBase<ViewExampleWithPaginationRequest>
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public EventListWithPaginationViewModel(IEventReadOnlyRepository eventReadOnlyRepository, ILocalizationService localizationService)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ViewExampleWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _eventReadOnlyRepository.GetAllEventsAsync(cancellationToken);

                Data = result.Data!;
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
                        Error = _localizationService["Error occurred while getting the list of Events"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToGet, "list of Events")
                    }
                };
            }
        }
    }
}
