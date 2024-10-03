using BaseSolution.Application.DataTransferObjects.Participant.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.ParticipantViewModel
{
    public class ParticipantDeleteViewModel : ViewModelBase<ParticipantDeleteRequest>
    {
        private readonly IParticipantReadWriteRepository _participantReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public ParticipantDeleteViewModel(IParticipantReadWriteRepository participantReadWriteRepository, ILocalizationService localizationService)
        {
            _participantReadWriteRepository = participantReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ParticipantDeleteRequest request, CancellationToken cancellationToken)
        {
            var result = await _participantReadWriteRepository.DeleteParticipantAsync(request.Id, cancellationToken);

            if (result.Success)
            {
                Success = true;
                Message = "Participant deleted successfully";
            }
            else
            {
                Success = false;
                ErrorItems = result.Errors;
                Message = "Participant deletion failed";
            }
        }
    }

}
