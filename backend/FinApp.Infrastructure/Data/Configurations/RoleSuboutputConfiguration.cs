using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class RoleSuboutputConfiguration : IEntityTypeConfiguration<RoleSuboutput>
{
    public void Configure(EntityTypeBuilder<RoleSuboutput> builder)
    {
        builder.ToTable("RoleSuboutputs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.Property(x => x.KodeSuboutput)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.CreatedAt);

        // Relationships
        builder.HasOne(x => x.Role)
            .WithMany(r => r.RoleSuboutputs)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite index to prevent duplicate suboutput per role
        builder.HasIndex(x => new { x.RoleId, x.KodeSuboutput })
            .IsUnique();
    }
}
