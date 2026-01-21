using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nama)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Satuan)
            .HasMaxLength(50);

        builder.Property(x => x.HargaSatuan)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Nama);
        builder.HasIndex(x => x.AkunId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Akun)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.AkunId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
