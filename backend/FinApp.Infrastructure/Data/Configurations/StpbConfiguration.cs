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

        // Reference fields
        builder.Property(x => x.KodeProgram).IsRequired().HasMaxLength(20);
        builder.Property(x => x.KodeKegiatan).IsRequired().HasMaxLength(20);
        builder.Property(x => x.KodeOutput).IsRequired().HasMaxLength(20);
        builder.Property(x => x.KodeSuboutput).IsRequired().HasMaxLength(20);
        builder.Property(x => x.KodeKomponen).IsRequired().HasMaxLength(20);
        builder.Property(x => x.KodeSubkomponen).IsRequired().HasMaxLength(20);
        builder.Property(x => x.KodeAkun).IsRequired().HasMaxLength(20);

        builder.Property(x => x.NomorSTPB)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.NomorSTPB)
            .IsUnique();

        builder.Property(x => x.Tanggal)
            .IsRequired();

        builder.HasIndex(x => x.Tanggal);

        builder.Property(x => x.Uraian)
            .HasColumnType("text");

        builder.Property(x => x.Nominal)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.PPn)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.PPh21)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.PPh22)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.PPh23)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.NilaiBersih)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.00m);

        builder.Property(x => x.IsLocked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(x => x.IsLocked);

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.HasIndex(x => x.CreatedBy);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate();

        // Relationships
        builder.HasOne(x => x.Creator)
            .WithMany(x => x.StpbList)
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
