using BaseSolution.Application.DataTransferObjects.Event;
using BaseSolution.Application.ValueObjects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IEventReadOnlyRepository
    {
        Task<RequestResult<EventDto>> GetEventByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<IEnumerable<EventDto>>> GetAllEventsAsync(CancellationToken cancellationToken);
    }
}
