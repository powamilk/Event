using BaseSolution.Application.DataTransferObjects.Registration.Request;
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
    public class RegistrationListWithPaginationViewModel : ViewModelBase<ViewRegistrationWithPaginationRequest>
    {
        private readonly IRegistrationReadOnlyRepository _registrationReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public RegistrationListWithPaginationViewModel(IRegistrationReadOnlyRepository registrationReadOnlyRepository, ILocalizationService localizationService)
        {
            _registrationReadOnlyRepository = registrationReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ViewRegistrationWithPaginationRequest request, CancellationToken cancellationToken)
        {
            var result = await _registrationReadOnlyRepository.GetAllRegistrationsAsync(cancellationToken);

            if (result.Success)
            {
                Data = result.Data;
                Success = true;
            }
            else
            {
                Success = false;
                ErrorItems = result.Errors;
                Message = "Có lỗi xảy ra khi lấy danh sách đăng ký.";
            }
        }
    }
}
