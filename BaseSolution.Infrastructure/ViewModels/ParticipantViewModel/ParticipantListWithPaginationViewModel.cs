using BaseSolution.Application.DataTransferObjects.Participant.Request;
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

namespace BaseSolution.Infrastructure.ViewModels.ParticipantViewModel
{
    public class ParticipantListWithPaginationViewModel : ViewModelBase<ViewParticipantWithPaginationRequest>
    {
        private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public ParticipantListWithPaginationViewModel(IParticipantReadOnlyRepository participantReadOnlyRepository, ILocalizationService localizationService)
        {
            _participantReadOnlyRepository = participantReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ViewParticipantWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _participantReadOnlyRepository.GetAllParticipantsAsync(cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Data = result.Data;
                }
                else
                {
                    Success = false;
                    Message = result.Message;
                    ErrorItems = result.Errors;
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
                Message = "Có lỗi xảy ra khi lấy danh sách người tham gia.";
            }
        }
    }
}
