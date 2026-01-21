using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class OutputConfiguration : IEntityTypeConfiguration<Output>
{
    public void Configure(EntityTypeBuilder<Output> builder)
    {
        builder.ToTable("Outputs");

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
        builder.HasIndex(x => x.KegiatanId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Kegiatan)
            .WithMany(x => x.Outputs)
            .HasForeignKey(x => x.KegiatanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Suboutputs)
            .WithOne(x => x.Output)
            .HasForeignKey(x => x.OutputId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
