using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Review;
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.ReviewViewModel
{
    public class ReviewListWithPaginationViewModel : ViewModelBase<ViewReviewReviewWithPaginationRequest>
    {
        private readonly IReviewReadOnlyRepository _reviewReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public ReviewListWithPaginationViewModel(IReviewReadOnlyRepository reviewReadOnlyRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _reviewReadOnlyRepository = reviewReadOnlyRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ViewReviewReviewWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reviewReadOnlyRepository.GetAllReviewsAsync(cancellationToken);

                if (result.Success)
                {
                    Data = result.Data;
                    Success = true;
                }
                else
                {
                    Success = false;
                    Message = "Không tìm thấy danh sách đánh giá.";
                    ErrorItems = result.Errors;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Message = "Có lỗi xảy ra khi lấy danh sách đánh giá.";
                ErrorItems = new[]
                {
                new ErrorItem
                {
                    FieldName = "ReviewList",
                    Error = ex.Message
                }
            };
            }
        }
    }
}
