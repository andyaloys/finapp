using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class SubkomponenConfiguration : IEntityTypeConfiguration<Subkomponen>
{
    public void Configure(EntityTypeBuilder<Subkomponen> builder)
    {
        builder.ToTable("Subkomponens");

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
        builder.HasIndex(x => x.KomponenId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Komponen)
            .WithMany(x => x.Subkomponens)
            .HasForeignKey(x => x.KomponenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Akuns)
            .WithOne(x => x.Subkomponen)
            .HasForeignKey(x => x.SubkomponenId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
