using FluentValidation;
using HamedStack.CQRS.FluentValidation;

namespace Tributech.Application.Create;

public class CreateSensorCommandValidator : CommandValidator<CreateSensorCommand, Guid>
{
    public CreateSensorCommandValidator()
    {
        RuleFor(s => s.Name).NotNull().NotEmpty().Length(1, 100);
        RuleFor(s => s.Location).NotNull().NotEmpty().Length(1, 200);
        RuleFor(s => s.LowerWarningLimit).Must(lowerWarningLimit => lowerWarningLimit > 0);
        RuleFor(s => s.UpperWarningLimit).Must(upperWarningLimit => upperWarningLimit < 100);
    }
}