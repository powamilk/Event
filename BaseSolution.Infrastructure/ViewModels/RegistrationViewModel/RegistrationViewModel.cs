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

namespace BaseSolution.Infrastructure.ViewModels.RegistrationViewModel
{
    public class RegistrationViewModel : ViewModelBase<int>
    {
        private readonly IRegistrationReadOnlyRepository _registrationReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public RegistrationViewModel(IRegistrationReadOnlyRepository registrationReadOnlyRepository, ILocalizationService localizationService)
        {
            _registrationReadOnlyRepository = registrationReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _registrationReadOnlyRepository.GetRegistrationByIdAsync(id, cancellationToken);

                Data = result.Data;
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
                    Error = _localizationService["Đã xảy ra lỗi khi lấy thông tin đăng ký."],
                    FieldName = LocalizationString.Common.FailedToGet + "Registration"
                }
            };
            }
        }
    }

}
