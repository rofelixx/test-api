using FluentValidation;
using Segfy.Schedule.Model.Dtos;
using Segfy.Schedule.Model.Entities;

namespace Segfy.Schedule.Validators
{
    public class ScheduleItemValidator : AbstractValidator<ScheduleItemDto>
    {
        public ScheduleItemValidator()
        {
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.Kind).NotEmpty();
            RuleFor(m => m.Recurrence).NotNull();
            RuleFor(m => m.Date).NotNull();
        }
    }
}
