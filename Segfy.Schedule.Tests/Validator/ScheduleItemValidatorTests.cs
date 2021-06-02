using FluentValidation.TestHelper;
using Bogus;
using Segfy.Schedule.Model.Dtos;
using Segfy.Schedule.Model.Enuns;
using Segfy.Schedule.Validators;
using Xunit;

namespace Segfy.Schedule.Tests.Validator
{
    public class ScheduleItemValidatorTests
    {
        [Fact]
        public void ScheduleItemValidator_ShouldValidateDescriptionAsRequired()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Date, f => f.Date.Future(1))
                .RuleFor(x => x.Recurrence, f => f.PickRandom<Recurrence>().ToString())
                .RuleFor(x => x.Type, f => f.PickRandom<ScheduleTypes>().ToString());
            var creation = generator.Generate();
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldHaveValidationErrorFor(schedule => schedule.Description);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Date);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Recurrence);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Type);
        }

        [Fact]
        public void ScheduleItemValidator_ShouldValidateDateAsRequired()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Recurrence, f => f.PickRandom<Recurrence>().ToString())
                .RuleFor(x => x.Type, f => f.PickRandom<ScheduleTypes>().ToString());
            var creation = generator.Generate();
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldHaveValidationErrorFor(schedule => schedule.Date);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Description);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Recurrence);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Type);
        }

        [Fact]
        public void ScheduleItemValidator_ShouldValidateTypeAsRequired()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Date, f => f.Date.Future(1))
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Recurrence, f => f.PickRandom<Recurrence>().ToString());
            var creation = generator.Generate();
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Date);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Description);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Recurrence);
            result.ShouldHaveValidationErrorFor(schedule => schedule.Type);
        }

        [Fact]
        public void ScheduleItemValidator_ShouldValidateRecurrenceAsRequired()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Date, f => f.Date.Future(1))
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Type, f => f.PickRandom<ScheduleTypes>().ToString());
            var creation = generator.Generate();
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Date);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Description);
            result.ShouldHaveValidationErrorFor(schedule => schedule.Recurrence);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Type);
        }

        [Fact]
        public void ScheduleItemValidator_ShouldValidateRecurrenceAsValidEnum()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Date, f => f.Date.Future(1))
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Type, f => f.PickRandom<ScheduleTypes>().ToString())
                .RuleFor(x => x.Recurrence, f => f.PickRandom<Recurrence>().ToString());
            var creation = generator.Generate();
            creation.Recurrence = "abc";
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Date);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Description);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Type);
            result.ShouldHaveValidationErrorFor(schedule => schedule.Recurrence);
        }

        [Fact]
        public void ScheduleItemValidator_ShouldValidateTypeAsValidEnum()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Date, f => f.Date.Future(1))
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Type, f => f.PickRandom<ScheduleTypes>().ToString())
                .RuleFor(x => x.Recurrence, f => f.PickRandom<Recurrence>().ToString());
            var creation = generator.Generate();
            creation.Type = "abc";
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Date);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Description);
            result.ShouldHaveValidationErrorFor(schedule => schedule.Type);
            result.ShouldNotHaveValidationErrorFor(schedule => schedule.Recurrence);
        }

        [Fact]
        public void ScheduleItemValidator_ShouldPassAllValidationsWhenModelIsValid()
        {
            //Given
            var generator = new Faker<ScheduleCreationDto>()
                .RuleFor(x => x.Date, f => f.Date.Future(1))
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Type, f => f.PickRandom<ScheduleTypes>().ToString())
                .RuleFor(x => x.Recurrence, f => f.PickRandom<Recurrence>().ToString());
            var creation = generator.Generate();
            var validator = new ScheduleItemValidator();

            //When
            var result = validator.TestValidate(creation);

            //Then
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}