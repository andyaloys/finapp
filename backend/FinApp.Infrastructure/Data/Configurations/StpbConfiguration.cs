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

        builder.Property(x => x.NomorSTPB)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.NomorSTPB)
            .IsUnique();

        builder.Property(x => x.Tanggal)
            .IsRequired();

        builder.HasIndex(x => x.Tanggal);

        builder.Property(x => x.Deskripsi)
            .HasColumnType("text");

        builder.Property(x => x.NilaiTotal)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Draft");

        builder.HasIndex(x => x.Status);

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.HasIndex(x => x.CreatedBy);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        // Relationships
        builder.HasOne(x => x.Creator)
            .WithMany(x => x.StpbList)
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
