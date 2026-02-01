using FinApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleSuboutput> RoleSuboutputs { get; set; }
    public DbSet<PpkBendahara> PpkBendaharas { get; set; }
    public DbSet<Stpb> Stpbs { get; set; }
    public DbSet<StpbDetail> StpbDetails { get; set; }
    public DbSet<Program> Programs { get; set; }
    public DbSet<Kegiatan> Kegiatans { get; set; }
    public DbSet<Output> Outputs { get; set; }
    public DbSet<Suboutput> Suboutputs { get; set; }
    public DbSet<Komponen> Komponens { get; set; }
    public DbSet<Subkomponen> Subkomponens { get; set; }
    public DbSet<Akun> Akuns { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<SequenceNumber> SequenceNumbers { get; set; }
    public DbSet<AnggaranMaster> AnggaranMasters { get; set; }

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
        // Seed default admin role
        var adminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000010");
        var userRoleId = Guid.Parse("00000000-0000-0000-0000-000000000011");
        
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = adminRoleId,
                Name = "Admin",
                Description = "Administrator dengan akses penuh",
                IsAdmin = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Role
            {
                Id = userRoleId,
                Name = "User",
                Description = "User biasa dengan akses terbatas",
                IsAdmin = false,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed default admin user
        // Password: Admin123! (will be hashed)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "admin",
                Email = "admin@finapp.com",
                FullName = "Administrator",
                RoleId = adminRoleId,
                // BCrypt hash for "Admin123!"
                PasswordHash = "$2a$11$VyFHd84rlCboUP.RPn25qeR7gw9i39bjj65fARIUvG6JkSjv.E2mW",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
