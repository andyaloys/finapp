using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Stpb> Stpbs { get; set; }
    public DbSet<Program> Programs { get; set; }
    public DbSet<Kegiatan> Kegiatans { get; set; }
    public DbSet<Output> Outputs { get; set; }
    public DbSet<Suboutput> Suboutputs { get; set; }
    public DbSet<Komponen> Komponens { get; set; }
    public DbSet<Subkomponen> Subkomponens { get; set; }
    public DbSet<Akun> Akuns { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<SequenceNumber> SequenceNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed default admin user
        // Password: Admin123! (will be hashed)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "admin",
                Email = "admin@finapp.com",
                FullName = "Administrator",
                Role = "Admin",
                // BCrypt hash for "Admin123!"
                PasswordHash = "$2a$11$6mtygzX7D/O53nh87B5W3O3ro/wBXjAF64kFyYrthx5vpWsg9vfmO",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
