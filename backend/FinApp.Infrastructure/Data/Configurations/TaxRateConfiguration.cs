using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.ToTable("TaxRates");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TaxCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(t => t.TaxCode)
            .IsUnique();

        builder.Property(t => t.TaxName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Rate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(t => t.IsActive);
    }
}
