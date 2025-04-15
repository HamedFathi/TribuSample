// ReSharper disable UnusedTypeParameter

using HamedStack.MiniMediator;
using HamedStack.TheResult;

namespace HamedStack.CQRS;

public interface IQuery<TResult> : IRequest<Result<TResult>>
{
}