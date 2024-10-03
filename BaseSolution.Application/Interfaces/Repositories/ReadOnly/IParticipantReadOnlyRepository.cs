using BaseSolution.Application.DataTransferObjects.Participant;
using BaseSolution.Application.ValueObjects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IParticipantReadOnlyRepository
    {
        Task<RequestResult<ParticipantDto>> GetParticipantByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<IEnumerable<ParticipantDto>>> GetAllParticipantsAsync(CancellationToken cancellationToken);
    }
}
