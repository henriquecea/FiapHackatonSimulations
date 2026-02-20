using FiapHackatonSimulations.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapHackatonSimulations.Infrastructure.Data.Mapping;

public class SensorDataMap : IEntityTypeConfiguration<SensorData>
{
    public void Configure(EntityTypeBuilder<SensorData> builder)
    {
        builder.ToTable("SensorData");

        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Plot)
            .HasColumnName("Plot")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(p => p.SoilMoisture)
            .HasColumnName("SoilMoisture")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.Temperature)
            .HasColumnName("Temperature")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PrecipitationLevel)
            .HasColumnName("PrecipitationLevel")
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}
