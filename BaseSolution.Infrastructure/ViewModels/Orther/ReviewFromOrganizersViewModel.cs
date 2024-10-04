using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
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

namespace BaseSolution.Infrastructure.ViewModels.Orther
{
    public class ReviewFromOrganizersViewModel : ViewModelBase<ReviewFromOrganizersRequest>
    {
        private readonly IReviewReadWriteRepository _reviewReadWriteRepository;
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public ReviewFromOrganizersViewModel(
            IReviewReadWriteRepository reviewReadWriteRepository,
            IEventReadOnlyRepository eventReadOnlyRepository,
            IParticipantReadOnlyRepository participantReadOnlyRepository,
            ILocalizationService localizationService,
            IMapper mapper)
        {
            _reviewReadWriteRepository = reviewReadWriteRepository;
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _participantReadOnlyRepository = participantReadOnlyRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ReviewFromOrganizersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var eventExists = await _eventReadOnlyRepository.GetEventByIdAsync(request.EventId, cancellationToken);
                if (!eventExists.Success || eventExists.Data == null)
                {
                    Success = false;
                    Message = _localizationService["Sự kiện không tồn tại."];
                    return;
                }
                var participantExists = await _participantReadOnlyRepository.GetParticipantByIdAsync(request.ParticipantId, cancellationToken);
                if (!participantExists.Success || participantExists.Data == null)
                {
                    Success = false;
                    Message = _localizationService["Người tham gia không tồn tại."];
                    return;
                }

                var reviewEntity = new Review
                {
                    EventId = request.EventId,
                    ParticipantId = request.ParticipantId,
                    Rating = request.Rating,
                    Comment = request.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _reviewReadWriteRepository.AddReviewAsync(reviewEntity, cancellationToken);

                if (result.Success)
                {
                    Data = result.Data;
                    Success = true;
                    Message = _localizationService["Đánh giá thành công."];
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = _localizationService["Có lỗi xảy ra khi thêm đánh giá."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = ex.InnerException?.Message ?? ex.Message,
                        FieldName = "AddReview"
                    }
                };
                Message = _localizationService["Có lỗi xảy ra khi thêm đánh giá."];
            }
        }
    }
}
