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
    public class ParticipantViewModel : ViewModelBase<int>
    {
        private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public ParticipantViewModel(IParticipantReadOnlyRepository participantReadOnlyRepository, ILocalizationService localizationService)
        {
            _participantReadOnlyRepository = participantReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _participantReadOnlyRepository.GetParticipantByIdAsync(id, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Data = result.Data;
                }
                else
                {
                    Success = false;
                    Message = "Không tìm thấy người tham gia.";
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
                Message = "Có lỗi xảy ra khi lấy thông tin người tham gia.";
            }
        }
    }
}
