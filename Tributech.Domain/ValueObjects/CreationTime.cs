using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;

namespace Tributech.Domain.ValueObjects;

public class CreationTime : SingleValueObject<DateTimeOffset>
{
    private CreationTime(DateTimeOffset value) : base(value) { }

    public static Result<CreationTime> Create(DateTimeOffset value)
    {
        if (value > DateTimeOffset.Now)
            return Result<CreationTime>.Failure($"{nameof(CreationTime)} cannot be greater than now.");
        return Result<CreationTime>.Success(new CreationTime(value));
    }
}