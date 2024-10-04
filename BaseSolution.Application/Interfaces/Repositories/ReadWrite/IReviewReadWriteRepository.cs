using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IReviewReadWriteRepository
    {
        Task<RequestResult<int>> AddReviewAsync(Review review, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateReviewAsync(Review review, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteReviewAsync(int id, CancellationToken cancellationToken);
    }
}
