using BaseSolution.Application.DataTransferObjects.Registration.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
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
    public class BulkRegistrationViewModel : ViewModelBase<BulkRegistrationRequest>
    {
        private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public BulkRegistrationViewModel(IRegistrationReadWriteRepository registrationReadWriteRepository, ILocalizationService localizationService)
        {
            _registrationReadWriteRepository = registrationReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(BulkRegistrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var bulkResult = await _registrationReadWriteRepository.BulkRegisterParticipantsAsync(request.EventId, request.ParticipantIds, cancellationToken);

                if (bulkResult.Success)
                {
                    Data = bulkResult.Data;
                    Success = true;
                    Message = _localizationService[$"Đăng ký thành công cho {request.ParticipantIds.Count} người tham gia."];
                }
                else
                {
                    Success = false;
                    ErrorItems = bulkResult.Errors;
                    Message = _localizationService["Đăng ký thất bại."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[] { new ErrorItem { Error = ex.Message } };
                Message = _localizationService["Có lỗi xảy ra khi đăng ký nhiều người tham gia."];
            }
        }
    }
}
