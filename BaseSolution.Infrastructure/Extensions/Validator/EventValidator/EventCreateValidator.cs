using BaseSolution.Application.DataTransferObjects.Event.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validator.EventValidator
{
    public class EventCreateValidator : AbstractValidator<EventCreateRequest>
    {
        public EventCreateValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Tên sự kiện không được để trống.")
                .MaximumLength(255).WithMessage("Tên sự kiện tối đa 255 ký tự.");

            RuleFor(e => e.Description)
                .MaximumLength(1000).WithMessage("Mô tả sự kiện tối đa 1000 ký tự.");

            RuleFor(e => e.Location)
                .NotEmpty().WithMessage("Địa điểm tổ chức sự kiện không được để trống.")
                .MaximumLength(255).WithMessage("Địa điểm tổ chức sự kiện tối đa 255 ký tự.");

            RuleFor(e => e.StartTime)
                .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống.")
                .Must(startTime => startTime > DateTime.Now).WithMessage("Thời gian bắt đầu phải ở trong tương lai.");

            RuleFor(e => e.EndTime)
                .NotEmpty().WithMessage("Thời gian kết thúc không được để trống.")
                .GreaterThan(e => e.StartTime).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");

            RuleFor(e => e.MaxParticipants)
                .GreaterThan(0).WithMessage("Số lượng tối đa người tham gia phải là số nguyên dương.");
        }
    }
}
