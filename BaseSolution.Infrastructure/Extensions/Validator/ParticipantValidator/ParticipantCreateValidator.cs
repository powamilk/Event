using BaseSolution.Application.DataTransferObjects.Participant.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validator.ParticipantValidator
{
    public class ParticipantCreateValidator : AbstractValidator<ParticipantCreateRequest>
    {
        public ParticipantCreateValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Tên người tham gia không được để trống.")
                .MaximumLength(255).WithMessage("Tên người tham gia tối đa 255 ký tự.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Địa chỉ email không được để trống.")
                .EmailAddress().WithMessage("Địa chỉ email không hợp lệ.");

            RuleFor(p => p.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Số điện thoại không hợp lệ, cần có định dạng số hợp lệ (có thể bao gồm mã quốc gia).");

            RuleFor(p => p.RegisteredAt)
                .NotEmpty().WithMessage("Thời gian đăng ký không được để trống.")
                .Must(BeAValidDate).WithMessage("Thời gian đăng ký không hợp lệ.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Thời gian đăng ký không được ở trong tương lai.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
