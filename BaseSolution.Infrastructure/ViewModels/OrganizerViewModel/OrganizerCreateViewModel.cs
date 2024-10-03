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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.OrganizerViewModel
{
    public class OrganizerCreateViewModel : ViewModelBase<OrganizerCreateRequest>
    {
        private readonly IOrganizerReadWriteRepository _organizerReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public OrganizerCreateViewModel(IOrganizerReadWriteRepository organizerReadWriteRepository, ILocalizationService localizationService)
        {
            _organizerReadWriteRepository = organizerReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(OrganizerCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var organizerEntity = new Organizer
                {
                    Name = request.Name,
                    ContactEmail = request.ContactEmail,
                    Phone = request.Phone
                };

                var result = await _organizerReadWriteRepository.AddOrganizerAsync(organizerEntity, cancellationToken);

                if (result.Success)
                {
                    Data = result.Data;
                    Success = true;
                    Message = "Người tổ chức đã được tạo thành công.";
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = "Tạo người tổ chức thất bại.";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                new ErrorItem
                {
                    Error = _localizationService["Error occurred while creating the Organizer: " + ex.Message],
                    FieldName = "Organizer"
                }
            };
            }
        }
    }

}
