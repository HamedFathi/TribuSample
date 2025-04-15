using HamedStack.CQRS;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace HamedStack.WebIntegrationTest;

/// <summary>
/// An abstract base class for web integration tests, providing access to an <see cref="HttpClient"/>, 
/// a CQRS <see cref="ICommandQueryDispatcher"/>, and the application's <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TStartup">The startup class of the ASP.NET Core application being tested.</typeparam>
/// <typeparam name="TDbContext">The Entity Framework <see cref="DbContext"/> used by the application.</typeparam>
public abstract class WebIntegrationTestBase<TStartup, TDbContext> : IClassFixture<WebApplicationFactory<TStartup>>
    where TStartup : class
    where TDbContext : DbContext
{
    /// <summary>
    /// Gets the <see cref="HttpClient"/> used to send HTTP requests to the test server.
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <summary>
    /// Gets the CQRS dispatcher used to send commands and queries directly to the application.
    /// </summary>
    protected ICommandQueryDispatcher Dispatcher { get; }

    /// <summary>
    /// Gets the application's <see cref="DbContext"/> for database access within the test.
    /// </summary>
    protected TDbContext DbContext { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebIntegrationTestBase{TStartup, TDbContext}"/> class.
    /// Creates a scoped service provider from the test server and resolves required services.
    /// </summary>
    /// <param name="factory">The <see cref="WebApplicationFactory{TStartup}"/> used to bootstrap the application for testing.</param>
    protected WebIntegrationTestBase(WebApplicationFactory<TStartup> factory)
    {
        var scope = factory.Services.CreateScope();
        Dispatcher = scope.ServiceProvider.GetRequiredService<ICommandQueryDispatcher>();
        DbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        HttpClient = factory.CreateClient();
    }
}
