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
    public class OrganizerViewModel : ViewModelBase<int>
    {
        private readonly IOrganizerReadOnlyRepository _organizerReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public OrganizerViewModel(IOrganizerReadOnlyRepository organizerReadOnlyRepository, ILocalizationService localizationService)
        {
            _organizerReadOnlyRepository = organizerReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _organizerReadOnlyRepository.GetOrganizerByIdAsync(id, cancellationToken);

                if (result.Success)
                {
                    Data = result.Data;
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
                        Error = _localizationService["Error occurred while fetching the Organizer details"],
                        FieldName = LocalizationString.Common.FailedToGet + "Organizer"
                    }
                };
            }
        }
    }
}
