// ReSharper disable UnusedTypeParameter


using HamedStack.MiniMediator;
using HamedStack.TheResult;

namespace HamedStack.CQRS;

public interface ICommand<TResult> : IRequest<Result<TResult>>, IBaseCommand
{
}

public interface ICommand : IRequest<Result>, IBaseCommand
{
}