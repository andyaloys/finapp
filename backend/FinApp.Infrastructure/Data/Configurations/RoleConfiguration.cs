using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinApp.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.IsAdmin)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt);

        // Relationships
        builder.HasMany(x => x.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.RoleSuboutputs)
            .WithOne(rs => rs.Role)
            .HasForeignKey(rs => rs.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
