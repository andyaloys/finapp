using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class KomponenConfiguration : IEntityTypeConfiguration<Komponen>
{
    public void Configure(EntityTypeBuilder<Komponen> builder)
    {
        builder.ToTable("Komponens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Kode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Nama)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Kode).IsUnique();
        builder.HasIndex(x => x.Nama);
        builder.HasIndex(x => x.SuboutputId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Suboutput)
            .WithMany(x => x.Komponens)
            .HasForeignKey(x => x.SuboutputId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Subkomponens)
            .WithOne(x => x.Komponen)
            .HasForeignKey(x => x.KomponenId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
