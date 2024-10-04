using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Database.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadWrite
{
    public class ReviewReadWriteRepository : IReviewReadWriteRepository
    {
        private readonly AppDbReadWriteContext _dbContext;

        public ReviewReadWriteRepository(AppDbReadWriteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<int>> AddReviewAsync(Review review, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Reviews.AddAsync(review, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(review.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Có lỗi xảy ra khi tạo đánh giá.", new[]
                {
                    new ErrorItem { FieldName = "AddReviewAsync", Error = ex.Message }
                });
            }
        }

        public async Task<RequestResult<int>> UpdateReviewAsync(Review review, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Reviews.Update(review);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(review.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Có lỗi xảy ra khi cập nhật đánh giá.", new[]
                {
                    new ErrorItem { FieldName = "UpdateReviewAsync", Error = ex.Message }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteReviewAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var review = await _dbContext.Reviews.FindAsync(new object[] { id }, cancellationToken);
                if (review == null)
                {
                    return RequestResult<int>.Fail("Không tìm thấy đánh giá.");
                }

                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Có lỗi xảy ra khi xóa đánh giá.", new[]
                {
                    new ErrorItem { FieldName = "DeleteReviewAsync", Error = ex.Message }
                });
            }
        }
    }
}
