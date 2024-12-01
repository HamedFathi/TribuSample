using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tributech.Domain;

namespace Tributech.Infrastructure
{
    public class SensorEntityTypeConfiguration : IEntityTypeConfiguration<Sensor>
    {
        public void Configure(EntityTypeBuilder<Sensor> builder)
        {
            builder.ToTable("Sensors");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.OwnsOne(e => e.Name, ba =>
            {
                ba.Property(t => t.Value).HasColumnName("Name").HasMaxLength(100).IsRequired();
            });

            builder.OwnsOne(e => e.Location, ba =>
            {
                ba.Property(t => t.Value).HasColumnName("Location").HasMaxLength(200).IsRequired();
            });

            builder.OwnsOne(e => e.CreationTime, ba =>
            {
                ba.Property(t => t.Value).HasColumnName("CreationTime").IsRequired();
            });

            builder.OwnsOne(e => e.WarningLimit, ba =>
            {
                ba.Property(t => t.LowerWarningLimit).HasPrecision(18, 2).HasColumnName("LowerWarningLimit").IsRequired();
                ba.Property(t => t.UpperWarningLimit).HasPrecision(18, 2).HasColumnName("UpperWarningLimit").IsRequired();
            });
        }
    }
}
