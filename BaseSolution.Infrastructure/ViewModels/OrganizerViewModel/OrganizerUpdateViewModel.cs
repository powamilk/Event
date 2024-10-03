using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Organizer.Request;
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

namespace BaseSolution.Infrastructure.ViewModels.OrganizerViewModel
{
    public class OrganizerUpdateViewModel : ViewModelBase<OrganizerUpdateRequest>
    {
        private readonly IOrganizerReadWriteRepository _organizerReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public OrganizerUpdateViewModel(IOrganizerReadWriteRepository organizerReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _organizerReadWriteRepository = organizerReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(OrganizerUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var organizerEntity = _mapper.Map<Organizer>(request);
                var result = await _organizerReadWriteRepository.UpdateOrganizerAsync(organizerEntity, cancellationToken);

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
                        Error = _localizationService["Error occurred while updating the Organizer"],
                        FieldName = LocalizationString.Common.FailedToUpdate + "Organizer"
                    }
                };
            }
        }
    }
}
