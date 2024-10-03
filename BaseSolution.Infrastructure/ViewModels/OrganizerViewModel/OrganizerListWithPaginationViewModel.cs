using BaseSolution.Application.DataTransferObjects.Organizer.Request;
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

namespace BaseSolution.Infrastructure.ViewModels.OrganizerViewModel
{
    public class OrganizerListWithPaginationViewModel : ViewModelBase<ViewOrganizerWithPaginationRequest>
    {
        private readonly IOrganizerReadOnlyRepository _organizerReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public OrganizerListWithPaginationViewModel(IOrganizerReadOnlyRepository organizerReadOnlyRepository, ILocalizationService localizationService)
        {
            _organizerReadOnlyRepository = organizerReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ViewOrganizerWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _organizerReadOnlyRepository.GetAllOrganizersAsync(cancellationToken);

                if (result.Success)
                {
                    var pagedData = result.Data
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToList();

                    Success = true;
                    Data = pagedData;
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while fetching the organizer list with pagination"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToGet, "list of organizers with pagination")
                    }
                };
            }
        }
    }
}
