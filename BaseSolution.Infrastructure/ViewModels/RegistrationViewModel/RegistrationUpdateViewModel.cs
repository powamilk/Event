using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Registration.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
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

namespace BaseSolution.Infrastructure.ViewModels.RegistrationViewModel
{
    public class RegistrationUpdateViewModel : ViewModelBase<RegistrationUpdateRequest>
    {
        private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
        private readonly IRegistrationReadOnlyRepository _registrationReadOnlyRepository;
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public RegistrationUpdateViewModel(
            IRegistrationReadWriteRepository registrationReadWriteRepository,
            IRegistrationReadOnlyRepository registrationReadOnlyRepository,
            IEventReadOnlyRepository eventReadOnlyRepository,
            IParticipantReadOnlyRepository participantReadOnlyRepository,
            ILocalizationService localizationService,
            IMapper mapper)
        {
            _registrationReadWriteRepository = registrationReadWriteRepository;
            _registrationReadOnlyRepository = registrationReadOnlyRepository;
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _participantReadOnlyRepository = participantReadOnlyRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(RegistrationUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingRegistration = await _registrationReadOnlyRepository.GetRegistrationByIdAsync(request.Id, cancellationToken);

                if (!existingRegistration.Success || existingRegistration.Data == null)
                {
                    Success = false;
                    Message = _localizationService["Không tìm thấy đăng ký."];
                    return;
                }
                var eventExists = await _eventReadOnlyRepository.GetEventByIdAsync(request.EventId, cancellationToken);
                if (eventExists == null)
                {
                    Success = false;
                    Message = _localizationService["Sự kiện không tồn tại."];
                    return;
                }

                var participantExists = await _participantReadOnlyRepository.GetParticipantByIdAsync(request.ParticipantId, cancellationToken);
                if (participantExists == null)
                {
                    Success = false;
                    Message = _localizationService["Người tham gia không tồn tại."];
                    return;
                }
                var registrationEntity = _mapper.Map<Registration>(request);
                var result = await _registrationReadWriteRepository.UpdateRegistrationAsync(registrationEntity, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Message = _localizationService["Cập nhật đăng ký thành công."];
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = _localizationService["Có lỗi xảy ra khi cập nhật đăng ký."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
            new ErrorItem
            {
                Error = ex.InnerException?.Message ?? ex.Message,
                FieldName = "UpdateRegistrationAsync"
            }
        };
                Message = _localizationService["Có lỗi xảy ra khi cập nhật đăng ký."];
            }
        }

    }
}
