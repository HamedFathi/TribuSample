using HamedStack.CQRS;
using Microsoft.Extensions.DependencyInjection;
using Tributech.Infrastructure;

namespace Tributech.IntegrationTests;

public abstract class WebIntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>
{
    public HttpClient HttpClient { get; }
    protected ICommandQueryDispatcher Dispatcher { get; }
    protected TributechDbContext DbContext { get; }

    protected WebIntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();
        Dispatcher = scope.ServiceProvider.GetRequiredService<ICommandQueryDispatcher>();
        DbContext = scope.ServiceProvider.GetRequiredService<TributechDbContext>();
        HttpClient = factory.CreateClient();
    }
}