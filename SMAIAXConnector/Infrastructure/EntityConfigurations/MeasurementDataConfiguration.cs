using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMAIAXConnector.Domain;

namespace SMAIAXConnector.Infrastructure.EntityConfigurations;

public class MeasurementDataConfiguration : IEntityTypeConfiguration<MeasurementData>
{
    public void Configure(EntityTypeBuilder<MeasurementData> builder)
    {
        
        // Define Id as the primary key with auto-increment
        builder.HasKey(e => new { e.Id, e.Timestamp });
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.SmartMeterId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Timestamp)
            .IsRequired();

        builder.Property(e => e.Uptime)
            .HasMaxLength(50)
            .IsRequired();

        // Configure columns with precision for numeric values
        builder.Property(e => e.PositiveActivePower).HasPrecision(18, 2);
        builder.Property(e => e.PositiveActiveEnergyTotal).HasPrecision(18, 2);
        builder.Property(e => e.NegativeActivePower).HasPrecision(18, 2);
        builder.Property(e => e.NegativeActiveEnergyTotal).HasPrecision(18, 2);
        builder.Property(e => e.ReactiveEnergyQuadrant1Total).HasPrecision(18, 2);
        builder.Property(e => e.ReactiveEnergyQuadrant3Total).HasPrecision(18, 2);
        builder.Property(e => e.TotalPower).HasPrecision(18, 2);
        builder.Property(e => e.CurrentPhase1).HasPrecision(18, 2);
        builder.Property(e => e.VoltagePhase1).HasPrecision(18, 2);
        builder.Property(e => e.CurrentPhase2).HasPrecision(18, 2);
        builder.Property(e => e.VoltagePhase2).HasPrecision(18, 2);
        builder.Property(e => e.CurrentPhase3).HasPrecision(18, 2);
        builder.Property(e => e.VoltagePhase3).HasPrecision(18, 2);
    }
}