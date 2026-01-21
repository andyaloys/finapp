using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class KegiatanConfiguration : IEntityTypeConfiguration<Kegiatan>
{
    public void Configure(EntityTypeBuilder<Kegiatan> builder)
    {
        builder.ToTable("Kegiatans");

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
        builder.HasIndex(x => x.ProgramId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Program)
            .WithMany(x => x.Kegiatans)
            .HasForeignKey(x => x.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Outputs)
            .WithOne(x => x.Kegiatan)
            .HasForeignKey(x => x.KegiatanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
