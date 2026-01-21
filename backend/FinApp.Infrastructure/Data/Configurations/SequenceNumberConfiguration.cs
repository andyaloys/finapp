using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class SequenceNumberConfiguration : IEntityTypeConfiguration<SequenceNumber>
{
    public void Configure(EntityTypeBuilder<SequenceNumber> builder)
    {
        builder.ToTable("SequenceNumbers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.EntityType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Year)
            .IsRequired();

        builder.Property(s => s.LastNumber)
            .IsRequired();

        // Unique constraint untuk EntityType + Year
        builder.HasIndex(s => new { s.EntityType, s.Year })
            .IsUnique();
    }
}
