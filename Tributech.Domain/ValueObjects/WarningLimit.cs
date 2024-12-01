using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;

namespace Tributech.Domain.ValueObjects;

public class WarningLimit : ValueObject
{
    public double UpperWarningLimit { get; }
    public double LowerWarningLimit { get; }

    private WarningLimit(double lowerWarningLimit, double upperWarningLimit)
    {
        LowerWarningLimit = lowerWarningLimit;
        UpperWarningLimit = upperWarningLimit;
    }

    public static Result<WarningLimit> Create(double lowerWarningLimit, double upperWarningLimit)
    {
        if (lowerWarningLimit < 0)
            return Result<WarningLimit>.Failure($"{nameof(lowerWarningLimit)} cannot be less than zero.");

        if (upperWarningLimit > 100)
            return Result<WarningLimit>.Failure($"{nameof(upperWarningLimit)} cannot be more than 100.");

        if (lowerWarningLimit > upperWarningLimit)
            return Result<WarningLimit>.Failure($"{nameof(lowerWarningLimit)} cannot be greater than {nameof(upperWarningLimit)}.");

        return Result<WarningLimit>.Success(new WarningLimit(lowerWarningLimit, upperWarningLimit));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return LowerWarningLimit;
        yield return UpperWarningLimit;
    }
}