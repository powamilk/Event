using BaseSolution.Application.DataTransferObjects.Registration.Request;
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

namespace BaseSolution.Infrastructure.ViewModels.RegistrationViewModel
{
    public class RegistrationCreateViewModel : ViewModelBase<RegistrationCreateRequest>
    {
        private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public RegistrationCreateViewModel(IRegistrationReadWriteRepository registrationReadWriteRepository, ILocalizationService localizationService)
        {
            _registrationReadWriteRepository = registrationReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(RegistrationCreateRequest request, CancellationToken cancellationToken)
        {
            var registration = new Registration
            {
                EventId = request.EventId,
                ParticipantId = request.ParticipantId,
                RegistrationDate = request.RegistrationDate,
                Status = request.Status
            };

            var result = await _registrationReadWriteRepository.AddRegistrationAsync(registration, cancellationToken);

            if (result.Success)
            {
                Data = result.Data;
                Success = true;
            }
            else
            {
                Success = false;
                ErrorItems = result.Errors;
                Message = "Tạo đăng ký thất bại.";
            }
        }
    }
}
