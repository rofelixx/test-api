using System;
using System.Linq;
using FluentValidation;
using Segfy.Schedule.Model.Dtos;
using Segfy.Schedule.Model.Enuns;

namespace Segfy.Schedule.Validators
{
    public static class ScheduleItemValidatorMessages
    {
        public static string GenerateMessageForRequired(string fieldName)
        {
            return $"O campo '{fieldName}' deve ser preenchido.";
        }

        public static string GenerateMessageForEnum(Type enumType, string fieldName)
        {
            var items = string.Join(',', Enum.GetNames(enumType).Select(x => x.ToLower()));
            return $"O campo '{fieldName}' deve ser um dos seguintes valores: [${items}]";
        }
    }

    public class ScheduleItemValidator : AbstractValidator<ScheduleCreationDto>
    {
        public ScheduleItemValidator()
        {
            RuleFor(m => m.Description)
                .NotEmpty()
                .WithMessage(ScheduleItemValidatorMessages.GenerateMessageForRequired("description"));

            RuleFor(m => m.Type)
                .NotEmpty()
                .WithMessage(ScheduleItemValidatorMessages.GenerateMessageForRequired("type"));

            RuleFor(m => m.Type)
                .Must(x => Enum.TryParse(typeof(ScheduleTypes), x, true, out _))
                .WithMessage(ScheduleItemValidatorMessages.GenerateMessageForEnum(typeof(ScheduleTypes), "type"));

            RuleFor(m => m.Recurrence)
                .NotNull()
                .WithMessage(ScheduleItemValidatorMessages.GenerateMessageForRequired("recurrence"));

            RuleFor(m => m.Recurrence)
                .Must(x => Enum.TryParse(typeof(Recurrence), x, true, out _))
                .WithMessage(ScheduleItemValidatorMessages.GenerateMessageForEnum(typeof(Recurrence), "type"));

            RuleFor(m => m.Date)
                .NotNull()
                .WithMessage(ScheduleItemValidatorMessages.GenerateMessageForRequired("date"));
        }

    }
}