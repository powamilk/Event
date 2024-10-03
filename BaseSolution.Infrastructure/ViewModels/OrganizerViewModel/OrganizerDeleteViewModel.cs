using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.OrganizerViewModel
{
    public class OrganizerDeleteViewModel : ViewModelBase<OrganizerDeleteRequest>
    {
        private readonly IOrganizerReadWriteRepository _organizerReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public OrganizerDeleteViewModel(IOrganizerReadWriteRepository organizerReadWriteRepository, ILocalizationService localizationService)
        {
            _organizerReadWriteRepository = organizerReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(OrganizerDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _organizerReadWriteRepository.DeleteOrganizerAsync(request.Id, cancellationToken);

                if (result.Success)
                {
                    Success = true;
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
                        Error = _localizationService["Error occurred while deleting the Organizer"],
                        FieldName = LocalizationString.Common.FailedToDelete + "Organizer"
                    }
                };
            }
        }
    }
}
