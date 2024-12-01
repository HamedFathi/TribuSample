using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;

namespace Tributech.Domain.ValueObjects;

public class Text : SingleValueObject<string>
{
    private Text(string value) : base(value) { }

    public static Result<Text> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Text>.Failure($"{nameof(Text)} cannot be null or whitespace.");
        return Result<Text>.Success(new Text(value));
    }
}