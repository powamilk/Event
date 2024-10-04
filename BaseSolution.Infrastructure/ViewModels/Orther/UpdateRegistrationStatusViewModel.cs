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
    public class UpdateRegistrationStatusViewModel : ViewModelBase<UpdateRegistrationStatusRequest>
    {
        private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public UpdateRegistrationStatusViewModel(IRegistrationReadWriteRepository registrationReadWriteRepository, ILocalizationService localizationService)
        {
            _registrationReadWriteRepository = registrationReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(UpdateRegistrationStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var updateResult = await _registrationReadWriteRepository.UpdateRegistrationStatusAsync(request.Id, request.Status, cancellationToken);

                if (updateResult.Success)
                {
                    Data = updateResult.Data;
                    Success = true;
                    Message = _localizationService["Cập nhật trạng thái đăng ký thành công."];
                }
                else
                {
                    Success = false;
                    ErrorItems = updateResult.Errors;
                    Message = _localizationService["Cập nhật trạng thái đăng ký thất bại."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[] { new ErrorItem { Error = ex.Message } };
                Message = _localizationService["Có lỗi xảy ra khi cập nhật trạng thái đăng ký."];
            }
        }
    }
}
