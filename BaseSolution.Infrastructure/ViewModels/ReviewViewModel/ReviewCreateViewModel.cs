using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Review;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.ReviewViewModel
{
    public class ReviewCreateViewModel : ViewModelBase<ReviewCreateRequest>
    {
        private readonly IReviewReadWriteRepository _reviewReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public ReviewCreateViewModel(IReviewReadWriteRepository reviewReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _reviewReadWriteRepository = reviewReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ReviewCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var reviewEntity = _mapper.Map<Review>(request); 

                var result = await _reviewReadWriteRepository.AddReviewAsync(reviewEntity, cancellationToken);

                if (result.Success)
                {
                    var reviewDto = _mapper.Map<ReviewDto>(reviewEntity); 
                    Data = reviewDto; 
                    Success = true;
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = _localizationService["Có lỗi xảy ra khi tạo đánh giá."];
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
                    FieldName = "CreateReview"
                }
            };
                Message = _localizationService["Có lỗi xảy ra khi tạo đánh giá."];
            }
        }
    }

}
