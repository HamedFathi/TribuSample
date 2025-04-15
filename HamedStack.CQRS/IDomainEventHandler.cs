using HamedStack.MiniMediator;
using HamedStack.TheAggregateRoot.Events;

namespace HamedStack.CQRS;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : DomainEvent
{
}