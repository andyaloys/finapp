using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class SuboutputConfiguration : IEntityTypeConfiguration<Suboutput>
{
    public void Configure(EntityTypeBuilder<Suboutput> builder)
    {
        builder.ToTable("Suboutputs");

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
        builder.HasIndex(x => x.OutputId);
        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.Output)
            .WithMany(x => x.Suboutputs)
            .HasForeignKey(x => x.OutputId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Komponens)
            .WithOne(x => x.Suboutput)
            .HasForeignKey(x => x.SuboutputId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
