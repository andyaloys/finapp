using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class StpbConfiguration : IEntityTypeConfiguration<Stpb>
{
    public void Configure(EntityTypeBuilder<Stpb> builder)
    {
        builder.ToTable("STPB");

        builder.HasKey(x => x.Id);

        // Header fields
        builder.Property(x => x.NomorSTPB)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.NomorSTPB)
            .IsUnique();

        builder.Property(x => x.TanggalSTPB)
            .IsRequired();

        builder.HasIndex(x => x.TanggalSTPB);

        builder.Property(x => x.Tahun)
            .IsRequired();

        builder.HasIndex(x => x.Tahun);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasIndex(x => x.Status);

        builder.Property(x => x.PpkBendaharaId)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.HasIndex(x => x.CreatedBy);

        builder.Property(x => x.TotalNilai)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.Keterangan)
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate();

        // Relationships
        builder.HasOne(x => x.PpkBendahara)
            .WithMany(x => x.Stpbs)
            .HasForeignKey(x => x.PpkBendaharaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Creator)
            .WithMany(x => x.StpbList)
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.StpbDetails)
            .WithOne(x => x.Stpb)
            .HasForeignKey(x => x.StpbId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
