using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class PpkBendaharaConfiguration : IEntityTypeConfiguration<PpkBendahara>
{
    public void Configure(EntityTypeBuilder<PpkBendahara> builder)
    {
        builder.ToTable("PpkBendahara");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nama)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Nama);

        builder.Property(x => x.NIP)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.NIP)
            .IsUnique();

        builder.Property(x => x.Jabatan)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(x => x.IsActive);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate();
    }
}
