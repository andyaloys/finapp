using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class AnggaranMasterConfiguration : IEntityTypeConfiguration<AnggaranMaster>
{
    public void Configure(EntityTypeBuilder<AnggaranMaster> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TahunAnggaran).IsRequired();
        builder.Property(x => x.Revisi).IsRequired();

        // Decimal fields with 2 decimal places
        builder.Property(x => x.HargaSat)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Pagu)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.HasilReviuKonsolidasiBaru)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Netto)
            .HasColumnType("decimal(18,2)");

        builder.ToTable("AnggaranMasters");
    }
}
