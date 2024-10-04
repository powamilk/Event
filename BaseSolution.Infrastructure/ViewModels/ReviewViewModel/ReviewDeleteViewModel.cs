using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.ReviewViewModel
{
    public class ReviewDeleteViewModel : ViewModelBase<int>
    {
        private readonly IReviewReadWriteRepository _reviewReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public ReviewDeleteViewModel(IReviewReadWriteRepository reviewReadWriteRepository, ILocalizationService localizationService)
        {
            _reviewReadWriteRepository = reviewReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reviewReadWriteRepository.DeleteReviewAsync(id, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Message = _localizationService["Xóa đánh giá thành công."];
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = _localizationService["Xóa đánh giá thất bại."];
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
                        FieldName = "DeleteReview"
                    }
                };
                Message = _localizationService["Có lỗi xảy ra khi xóa đánh giá."];
            }
        }
    }
}
