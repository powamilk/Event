using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validator.Organizer
{
    public class OrganizerCreateValidator : AbstractValidator<OrganizerCreateRequest>
    {
        public OrganizerCreateValidator()
        {
            RuleFor(o => o.Name)
                .NotEmpty().WithMessage("Tên người tổ chức không được để trống.")
                .MaximumLength(255).WithMessage("Tên người tổ chức tối đa 255 ký tự.");

            RuleFor(o => o.ContactEmail)
                .NotEmpty().WithMessage("Email liên hệ không được để trống.")
                .EmailAddress().WithMessage("Địa chỉ email không hợp lệ.");

            RuleFor(o => o.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Số điện thoại không hợp lệ, cần có định dạng số hợp lệ (có thể bao gồm mã quốc gia).");
        }
    }
}
