using BaseSolution.Application.DataTransferObjects.Review;
using BaseSolution.Application.ValueObjects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IReviewReadOnlyRepository
    {
        Task<RequestResult<IEnumerable<ReviewDto>>> GetAllReviewsAsync(CancellationToken cancellationToken);
        Task<RequestResult<ReviewDto>> GetReviewByIdAsync(int id, CancellationToken cancellationToken);
    }
}
