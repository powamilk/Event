using BaseSolution.Application.DataTransferObjects.Review.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validator.ReviewValidator
{
    public class ReviewUpdateValidator : AbstractValidator<ReviewUpdateRequest>
    {
        public ReviewUpdateValidator()
        {
            RuleFor(r => r.EventId)
                .NotEmpty().WithMessage("ID của sự kiện không được để trống.");

            RuleFor(r => r.ParticipantId)
                .NotEmpty().WithMessage("ID của người tham gia không được để trống.");

            RuleFor(r => r.Rating)
                .NotEmpty().WithMessage("Điểm đánh giá không được để trống.")
                .InclusiveBetween(1, 5).WithMessage("Điểm đánh giá phải nằm trong khoảng từ 1 đến 5.");

            RuleFor(r => r.Comment)
                .MaximumLength(1000).WithMessage("Nhận xét không được vượt quá 1000 ký tự.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
