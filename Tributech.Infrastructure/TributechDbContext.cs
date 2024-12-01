using HamedStack.TheRepository.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tributech.Domain;

namespace Tributech.Infrastructure;

public class TributechDbContext : DbContextBase
{
    public TributechDbContext(DbContextOptions<TributechDbContext> options, ILogger<DbContextBase> logger) : base(options, logger)
    {
    }

    public DbSet<Sensor> Sensors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SensorEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}