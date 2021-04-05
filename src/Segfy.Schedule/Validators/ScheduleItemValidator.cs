using FluentValidation;
using Segfy.Schedule.Model.Dtos;

namespace Segfy.Schedule.Validators
{
    public class ScheduleItemValidator : AbstractValidator<ScheduleCreationDto>
    {
        public ScheduleItemValidator()
        {
            RuleFor(m => m.Description).NotEmpty().WithMessage("Campo obrigatório");
            RuleFor(m => m.Type).NotEmpty().WithMessage("Campo obrigatório");
            RuleFor(m => m.Recurrence).NotNull().WithMessage("Campo obrigatório");
            RuleFor(m => m.Date).NotNull().WithMessage("Campo obrigatório");
        }
    }
}
