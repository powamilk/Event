using BaseSolution.Application.DataTransferObjects.Organizer;
using BaseSolution.Application.ValueObjects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IOrganizerReadOnlyRepository
    {
        Task<RequestResult<OrganizerDto?>> GetOrganizerByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<IEnumerable<OrganizerDto>>> GetAllOrganizersAsync(CancellationToken cancellationToken);
    }
}
