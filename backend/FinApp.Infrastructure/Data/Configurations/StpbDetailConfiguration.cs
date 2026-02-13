using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class StpbDetailConfiguration : IEntityTypeConfiguration<StpbDetail>
{
    public void Configure(EntityTypeBuilder<StpbDetail> builder)
    {
        builder.ToTable("StpbDetails");

        builder.HasKey(x => x.Id);

        // Foreign Key
        builder.Property(x => x.StpbId)
            .IsRequired();

        builder.HasIndex(x => x.StpbId);

        // Denormalized Anggaran Structure
        builder.Property(x => x.Tahun)
            .IsRequired();

        builder.Property(x => x.Revisi)
            .IsRequired();

        builder.Property(x => x.KodeProgram)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaProgram)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.KodeKegiatan)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaKegiatan)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.KodeOutput)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaOutput)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.KodeSuboutput)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaSuboutput)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(x => x.KodeSuboutput);

        builder.Property(x => x.KodeKomponen)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaKomponen)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.KodeSubkomponen)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaSubkomponen)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.KodeAkun)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.NamaAkun)
            .IsRequired()
            .HasMaxLength(500);

        // Item (Optional)
        builder.Property(x => x.NoItem)
            .HasMaxLength(20);

        builder.Property(x => x.NamaItem)
            .HasMaxLength(500);

        // Transaction Details
        builder.Property(x => x.Volume)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Satuan)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.HargaSatuan)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.JumlahHarga)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Keterangan)
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate();

        // Relationships
        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(x => x.Penerima)
            .WithMany(p => p.Stpbs)
            .HasForeignKey(x => x.PenerimaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
