using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.ReviewViewModel
{
    public class ReviewUpdateViewModel : ViewModelBase<ReviewUpdateRequest>
    {
        private readonly IReviewReadWriteRepository _reviewReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public ReviewUpdateViewModel(IReviewReadWriteRepository reviewReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _reviewReadWriteRepository = reviewReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ReviewUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var reviewEntity = _mapper.Map<Review>(request);
                var result = await _reviewReadWriteRepository.UpdateReviewAsync(reviewEntity, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Message = _localizationService["Cập nhật đánh giá thành công."];
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = _localizationService["Cập nhật đánh giá thất bại."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "UpdateReview"
                    }
                };
                Message = _localizationService["Có lỗi xảy ra khi cập nhật đánh giá."];
            }
        }
    }
}
