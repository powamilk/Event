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

namespace BaseSolution.Infrastructure.ViewModels.RegistrationViewModel
{
    public class RegistrationDeleteViewModel : ViewModelBase<RegistrationDeleteRequest>
    {
        private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public RegistrationDeleteViewModel(IRegistrationReadWriteRepository registrationReadWriteRepository, ILocalizationService localizationService)
        {
            _registrationReadWriteRepository = registrationReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(RegistrationDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _registrationReadWriteRepository.DeleteRegistrationAsync(request.Id, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Message = "Xóa đăng ký thành công.";
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = "Không tìm thấy đăng ký để xóa.";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Có lỗi xảy ra trong quá trình xóa đăng ký."],
                        FieldName = "DeleteRegistration"
                    }
                };
                Message = ex.Message;
            }
        }
    }
}
