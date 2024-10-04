using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Review;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadOnly
{
    public class ReviewReadOnlyRepository : IReviewReadOnlyRepository
    {
        private readonly AppDbReadOnlyContext _dbContext;
        private readonly IMapper _mapper;

        public ReviewReadOnlyRepository(AppDbReadOnlyContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RequestResult<IEnumerable<ReviewDto>>> GetAllReviewsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var reviews = await _dbContext.Reviews
                    .Include(r => r.Event)
                    .Include(r => r.Participant)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                if (!reviews.Any())
                {
                    return RequestResult<IEnumerable<ReviewDto>>.Fail("Không tìm thấy danh sách đánh giá.");
                }

                var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
                return RequestResult<IEnumerable<ReviewDto>>.Succeed(reviewDtos);
            }
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<ReviewDto>>.Fail(ex.Message);
            }
        }


        public async Task<RequestResult<ReviewDto>> GetReviewByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var review = await _dbContext.Reviews
                    .Include(r => r.Event) 
                    .Include(r => r.Participant) 
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

                if (review == null)
                {
                    return RequestResult<ReviewDto>.Fail("Không tìm thấy đánh giá.");
                }

                var reviewDto = new ReviewDto
                {
                    Id = review.Id,
                    EventId = review.EventId,
                    EventName = review.Event.Name, 
                    ParticipantId = review.ParticipantId,
                    ParticipantName = review.Participant.Name, 
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreatedAt = review.CreatedAt
                };

                return RequestResult<ReviewDto>.Succeed(reviewDto);
            }
            catch (Exception ex)
            {
                return RequestResult<ReviewDto>.Fail("Có lỗi xảy ra khi lấy thông tin đánh giá.", new[]
                {
                    new ErrorItem { FieldName = "GetReviewByIdAsync", Error = ex.Message }
                });
            }
        }
    }
}
