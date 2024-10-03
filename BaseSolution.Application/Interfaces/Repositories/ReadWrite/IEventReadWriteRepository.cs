using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IEventReadWriteRepository
    {
        Task<RequestResult<int>> AddEventAsync(Event entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateEventAsync(Event entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteEventAsync(int id, CancellationToken cancellationToken);
    }
}
