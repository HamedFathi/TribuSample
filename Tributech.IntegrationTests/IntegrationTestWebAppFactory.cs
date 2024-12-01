using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tributech.Infrastructure;
using Tributech.Presentation;

namespace Tributech.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TributechDbContext>));

            services.AddDbContext<TributechDbContext>((_, options) =>
            {
                options.UseInMemoryDatabase("TributechDb");
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TributechDbContext>();

            db.Database.EnsureCreated();

            builder.UseEnvironment("Development");
        });
    }
}