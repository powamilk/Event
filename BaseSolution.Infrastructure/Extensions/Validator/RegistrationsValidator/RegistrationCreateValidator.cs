using BaseSolution.Application.DataTransferObjects.Registration.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validator.RegistrationsValidator
{
    public class RegistrationCreateValidator : AbstractValidator<RegistrationCreateRequest>
    {
        public RegistrationCreateValidator()
        {
            RuleFor(r => r.EventId)
                .NotEmpty().WithMessage("ID của sự kiện không được để trống.")
                .GreaterThan(0).WithMessage("ID của sự kiện phải lớn hơn 0.");

            RuleFor(r => r.ParticipantId)
                .NotEmpty().WithMessage("ID của người tham gia không được để trống.")
                .GreaterThan(0).WithMessage("ID của người tham gia phải lớn hơn 0.");

            RuleFor(r => r.RegistrationDate)
                .NotEmpty().WithMessage("Ngày đăng ký không được để trống.")
                .Must(BeAValidDate).WithMessage("Ngày đăng ký không hợp lệ.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Ngày đăng ký không được ở trong tương lai.");

            RuleFor(r => r.Status)
                .NotEmpty().WithMessage("Trạng thái đăng ký không được để trống.")
                .Must(status => status == "đã xác nhận" || status == "đã hủy")
                .WithMessage("Trạng thái chỉ được phép là 'đã xác nhận' hoặc 'đã hủy'.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
