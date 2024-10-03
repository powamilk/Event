using AutoMapper;
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
    public class ParticipantUpdateViewModel : ViewModelBase<ParticipantUpdateRequest>
    {
        private readonly IParticipantReadWriteRepository _participantReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public ParticipantUpdateViewModel(IParticipantReadWriteRepository participantReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _participantReadWriteRepository = participantReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ParticipantUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var participantEntity = _mapper.Map<Participant>(request);
                var result = await _participantReadWriteRepository.UpdateParticipantAsync(participantEntity, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Data = result.Data;
                    Message = "Cập nhật người tham gia thành công.";
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "Participant"
                    }
                };
                Message = "Có lỗi xảy ra khi cập nhật thông tin người tham gia.";
            }
        }
    }
}
