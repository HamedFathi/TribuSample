// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedTypeParameter

using HamedStack.MiniMediator;
using HamedStack.TheResult;

namespace HamedStack.CQRS;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>
{
}