// ReSharper disable UnusedTypeParameter
// ReSharper disable UnusedMember.Global

using HamedStack.MiniMediator;
using HamedStack.TheResult;

namespace HamedStack.CQRS;


public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>>, IBaseCommandHandler
    where TCommand : ICommand<TResult>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>, IBaseCommandHandler
    where TCommand : ICommand
{
}