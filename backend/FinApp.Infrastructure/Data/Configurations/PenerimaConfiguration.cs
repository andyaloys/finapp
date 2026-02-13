using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class PenerimaConfiguration : IEntityTypeConfiguration<Penerima>
{
    public void Configure(EntityTypeBuilder<Penerima> builder)
    {
        builder.ToTable("Penerimas");
        
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Nama)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Npwp)
            .HasMaxLength(20);
        
        builder.Property(p => p.Alamat)
            .HasMaxLength(500);
        
        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        
        // Index for faster searches
        builder.HasIndex(p => p.Nama);
        builder.HasIndex(p => p.IsActive);
    }
}
