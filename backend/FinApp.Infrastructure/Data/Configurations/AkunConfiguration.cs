using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class AkunConfiguration : IEntityTypeConfiguration<Akun>
{
    public void Configure(EntityTypeBuilder<Akun> builder)
    {
        builder.ToTable("Akuns");

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
        builder.HasIndex(x => x.SubkomponenId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Subkomponen)
            .WithMany(x => x.Akuns)
            .HasForeignKey(x => x.SubkomponenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Akun)
            .HasForeignKey(x => x.AkunId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
