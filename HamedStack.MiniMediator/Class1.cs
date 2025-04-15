// ReSharper disable UnusedMember.Global
// ReSharper disable UseCollectionExpression
// ReSharper disable UnusedMemberInSuper.Global

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HamedStack.MiniMediator;

/// <summary>
/// Marker interface for requests that do not return a response.
/// </summary>
public interface IRequest : IBaseRequest;

/// <summary>
/// Marker interface for requests that return a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the request handler.</typeparam>
public interface IRequest<out TResponse> : IBaseRequest;

/// <summary>
/// Base marker interface for all request types.
/// </summary>
public interface IBaseRequest;

/// <summary>
/// Marker interface for notification objects that will be sent to multiple handlers.
/// </summary>
public interface INotification;

/// <summary>
/// Mediator interface for sending requests and publishing notifications.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request to a single handler and returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of response expected from the request handler.</typeparam>
    /// <param name="request">The request object to send.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the handler's response.</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request to a single handler that does not return a response.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to send.</typeparam>
    /// <param name="request">The request object to send.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;

    /// <summary>
    /// Sends a request object to a single handler and returns a response object.
    /// This method uses dynamic dispatch and is useful when the request type is not known at compile time.
    /// </summary>
    /// <param name="request">The request object to send.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the handler's response, or null if the request does not return a response.</returns>
    Task<object?> Send(object request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification object to multiple handlers.
    /// This method uses dynamic dispatch and is useful when the notification type is not known at compile time.
    /// </summary>
    /// <param name="notification">The notification object to publish.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Publish(object notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to multiple handlers.
    /// </summary>
    /// <typeparam name="TNotification">The type of notification to publish.</typeparam>
    /// <param name="notification">The notification object to publish.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}

/// <summary>
/// Defines a handler for a request that does not return a response.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for a request that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request and returns a response.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the handler's response.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for a notification.
/// </summary>
/// <typeparam name="TNotification">The type of notification being handled.</typeparam>
public interface INotificationHandler<in TNotification> where TNotification : INotification
{
    /// <summary>
    /// Handles the specified notification.
    /// </summary>
    /// <param name="notification">The notification to handle.</param>
    /// <param name="cancellationToken">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a behavior to be executed in the request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response from the request.</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : notnull
{
    /// <summary>
    /// Handles the request by executing pipeline behavior and calling the next delegate.
    /// </summary>
    /// <param name="request">The request being handled.</param>
    /// <param name="next">The delegate that represents the next behavior or handler in the pipeline.</param>
    /// <param name="cancellationToken">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the handler's response.</returns>
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}

/// <summary>
/// Represents a delegate that returns a response from a request handler.
/// </summary>
/// <typeparam name="TResponse">The type of response from the request handler.</typeparam>
/// <returns>A task representing the asynchronous operation with the handler's response.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Default implementation of the <see cref="IMediator"/> interface.
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve handlers and behaviors.</param>
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Sends a request to a single handler and returns a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The type of response expected from the request handler.</typeparam>
    /// <param name="request">The request object to send.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the handler's response.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no handler is registered for the request type.</exception>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType)
                      ?? throw new InvalidOperationException($"Handler for {requestType.Name} not registered");
        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<IRequest<TResponse>, TResponse>>();
        RequestHandlerDelegate<TResponse> pipeline = () =>
        {
            var handleMethod = handlerType.GetMethod("Handle");
            return (Task<TResponse>)handleMethod?.Invoke(handler, new object[] { request, cancellationToken })!;
        };
        foreach (var behavior in behaviors.Reverse())
        {
            var currentPipeline = pipeline;
            pipeline = () => behavior.Handle(request, currentPipeline, cancellationToken);
        }
        return await pipeline();
    }

    /// <summary>
    /// Sends a request to a single handler that does not return a response.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to send.</typeparam>
    /// <param name="request">The request object to send.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no handler is registered for the request type.</exception>
    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
        var handler = _serviceProvider.GetService(handlerType)
                      ?? throw new InvalidOperationException($"Handler for {requestType.Name} not registered");
        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TRequest, Unit>>();
        RequestHandlerDelegate<Unit> pipeline = async () =>
        {
            var handleMethod = handlerType.GetMethod("Handle");
            await (Task)handleMethod?.Invoke(handler, new object[] { request, cancellationToken })!;
            return Unit.Value;
        };
        foreach (var behavior in behaviors.Reverse())
        {
            var currentPipeline = pipeline;
            pipeline = () => behavior.Handle(request, currentPipeline, cancellationToken);
        }
        await pipeline();
    }

    /// <summary>
    /// Sends a request object to a single handler and returns a response object.
    /// This method uses dynamic dispatch and is useful when the request type is not known at compile time.
    /// </summary>
    /// <param name="request">The request object to send.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the handler's response, or null if the request does not return a response.</returns>
    public async Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var requestInterfaceType = requestType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));
        if (requestInterfaceType != null)
        {
            var responseType = requestInterfaceType.GetGenericArguments()[0];
            var method = typeof(Mediator).GetMethod(nameof(Send),
                new[] { typeof(IRequest<>).MakeGenericType(responseType), typeof(CancellationToken) });
            var genericMethod = method?.MakeGenericMethod(responseType);
            return await (Task<object>)genericMethod?.Invoke(this, new[] { request, cancellationToken })!;
        }
        else
        {
            var method =
                typeof(Mediator).GetMethod(nameof(Send), new[] { typeof(IRequest), typeof(CancellationToken) });
            var genericMethod = method?.MakeGenericMethod(requestType);
            await (Task)genericMethod?.Invoke(this, new[] { request, cancellationToken })!;
            return null;
        }
    }

    /// <summary>
    /// Publishes a notification to multiple handlers.
    /// </summary>
    /// <typeparam name="TNotification">The type of notification to publish.</typeparam>
    /// <param name="notification">The notification object to publish.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Publish<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default) where TNotification : INotification
    {
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var handlers = _serviceProvider.GetServices(handlerType);
        var tasks = new List<Task>();
        foreach (var handler in handlers)
        {
            var handleMethod = handlerType.GetMethod("Handle");
            tasks.Add((Task)handleMethod?.Invoke(handler, new object[] { notification, cancellationToken })!);
        }
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Publishes a notification object to multiple handlers.
    /// This method uses dynamic dispatch and is useful when the notification type is not known at compile time.
    /// </summary>
    /// <param name="notification">The notification object to publish.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        var notificationType = notification.GetType();
        var method = typeof(Mediator).GetMethod(nameof(Publish),
            new[] { typeof(INotification), typeof(CancellationToken) });
        var genericMethod = method?.MakeGenericMethod(notificationType);
        await (Task)genericMethod?.Invoke(this, new[] { notification, cancellationToken })!;
    }
}

/// <summary>
/// Represents a void type, or a type with a single value.
/// </summary>
public struct Unit
{
    /// <summary>
    /// Gets the single value of the Unit type.
    /// </summary>
    /// <returns>The single value of the Unit type.</returns>
    public static Unit Value => new();
}

/// <summary>
/// Extension methods for registering mediator services with the dependency injection container.
/// </summary>
public static class MiniMediatorServiceCollectionExtensions
{
    /// <summary>
    /// Adds mediator services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="assemblies">The assemblies to scan for handler implementations. If not provided, all non-system assemblies in the current AppDomain will be scanned.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddMiniMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();
        if (assemblies.Length == 0)
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a is { IsDynamic: false, FullName: not null } && !a.FullName.StartsWith("System") && !a.FullName.StartsWith("Microsoft"))
                .ToArray();
        }
        foreach (var assembly in assemblies)
        {
            try
            {
                var requestHandlers = assembly.GetTypes()
                    .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                                t.GetInterfaces().Any(i =>
                                    i.IsGenericType &&
                                    (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))));
                foreach (var handler in requestHandlers)
                {
                    foreach (var handlerInterface in handler.GetInterfaces()
                        .Where(i => i.IsGenericType &&
                                  (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                                   i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))))
                    {
                        services.AddTransient(handlerInterface, handler);
                    }
                }
                var notificationHandlers = assembly.GetTypes()
                    .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                                t.GetInterfaces().Any(i =>
                                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)));
                foreach (var handler in notificationHandlers)
                {
                    foreach (var handlerInterface in handler.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                    {
                        services.AddTransient(handlerInterface, handler);
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Swallow exceptions from assemblies that cannot be loaded
            }
        }
        return services;
    }
}