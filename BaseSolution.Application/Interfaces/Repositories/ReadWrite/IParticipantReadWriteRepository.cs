using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IParticipantReadWriteRepository
    {
        Task<RequestResult<int>> AddParticipantAsync(Participant entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateParticipantAsync(Participant entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteParticipantAsync(int id, CancellationToken cancellationToken);
    }
}
