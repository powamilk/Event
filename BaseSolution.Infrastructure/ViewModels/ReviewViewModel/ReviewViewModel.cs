using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.ReviewViewModel
{
    public class ReviewViewModel : ViewModelBase<int>
    {
        private readonly IReviewReadOnlyRepository _reviewReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public ReviewViewModel(IReviewReadOnlyRepository reviewReadOnlyRepository, ILocalizationService localizationService)
        {
            _reviewReadOnlyRepository = reviewReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reviewReadOnlyRepository.GetReviewByIdAsync(id, cancellationToken);

                if (result.Success && result.Data != null)
                {
                    Data = result.Data;
                    Success = true;
                }
                else
                {
                    Success = false;
                    Message = _localizationService["Không tìm thấy đánh giá."];
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
                        FieldName = "ViewReview"
                    }
                };
                Message = _localizationService["Có lỗi xảy ra khi lấy thông tin đánh giá."];
            }
        }
    }
}
