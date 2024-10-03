using BaseSolution.Application.DataTransferObjects.Participant.Request;
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

namespace BaseSolution.Infrastructure.ViewModels.ParticipantViewModel
{
    public class ParticipantCreateViewModel : ViewModelBase<ParticipantCreateRequest>
    {
        private readonly IParticipantReadWriteRepository _participantReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public ParticipantCreateViewModel(IParticipantReadWriteRepository participantReadWriteRepository, ILocalizationService localizationService)
        {
            _participantReadWriteRepository = participantReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ParticipantCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var participantEntity = new Participant
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    RegisteredAt = request.RegisteredAt
                };
                var result = await _participantReadWriteRepository.AddParticipantAsync(participantEntity, cancellationToken);

                if (result.Success)
                {
                    Data = participantEntity; 
                    Success = true;
                    Message = _localizationService["Tạo người tham gia thành công."];
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = _localizationService["Tạo người tham gia thất bại."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Message = _localizationService["Đã xảy ra lỗi khi tạo người tham gia."];
                ErrorItems = new[]
                {
                new ErrorItem
                {
                    Error = ex.Message,
                    FieldName = "ParticipantCreateViewModel.HandleAsync"
                }
            };
            }
        }
    }


}
