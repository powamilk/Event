using BaseSolution.Application.DataTransferObjects.Registration;
using BaseSolution.Application.ValueObjects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IRegistrationReadOnlyRepository
    {
        Task<RequestResult<RegistrationDto?>> GetRegistrationByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<IEnumerable<RegistrationDto>>> GetAllRegistrationsAsync(CancellationToken cancellationToken);
    }
}
