
using HamedStack.CQRS.ServiceCollection;
using HamedStack.MiniMediator;
using HamedStack.TheRepository.EntityFrameworkCore.Outbox;
using HamedStack.TheRepository.ServiceCollection;
using HamedStack.TheResult.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Tributech.Application.Service;
using Tributech.Domain.Repositories;
using Tributech.Domain.Services;
using Tributech.Infrastructure;

namespace Tributech.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            builder.Services.AddTransient<ISensorDataRepository, SensorDataRepository>();
            builder.Services.AddTransient<ISensorDataService, SensorDataService>();

            builder.Services.AddDbContext<TributechDbContext>(options =>
                options.UseInMemoryDatabase("TributechDb"));

            builder.Services.AddInfrastructureServices<TributechDbContext>();
            builder.Services.AddApplicationServices();

            builder.Services.AddOutboxBackgroundService(options =>
            {
                options.PollingIntervalSeconds = 10;
                options.BatchSize = 10;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseResultException();

            app.Run();
        }
    }
}
