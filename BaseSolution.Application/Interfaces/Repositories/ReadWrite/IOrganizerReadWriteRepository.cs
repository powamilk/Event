using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IOrganizerReadWriteRepository
    {
        Task<RequestResult<int>> AddOrganizerAsync(Organizer entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateOrganizerAsync(Organizer entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteOrganizerAsync(int id, CancellationToken cancellationToken);
    }
}
