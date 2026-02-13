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
    public DbSet<Penerima> Penerimas { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    public DbSet<SequenceNumber> SequenceNumbers { get; set; }
    public DbSet<AnggaranMaster> AnggaranMasters { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }

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

        // Seed menus with fixed GUIDs
        var menu1 = Guid.Parse("00000000-0000-0000-0000-000000000021");
        var menu2 = Guid.Parse("00000000-0000-0000-0000-000000000022");
        var menu3 = Guid.Parse("00000000-0000-0000-0000-000000000023");
        var menu4 = Guid.Parse("00000000-0000-0000-0000-000000000024");
        var menu5 = Guid.Parse("00000000-0000-0000-0000-000000000025");
        var menu6 = Guid.Parse("00000000-0000-0000-0000-000000000026");
        var menu7 = Guid.Parse("00000000-0000-0000-0000-000000000027");
        var menu8 = Guid.Parse("00000000-0000-0000-0000-000000000028");
        var menu9 = Guid.Parse("00000000-0000-0000-0000-000000000029");
        var menu10 = Guid.Parse("00000000-0000-0000-0000-000000000030");

        modelBuilder.Entity<Menu>().HasData(
            new Menu { Id = menu1, Key = "transaksi", Label = "Transaksi", Icon = "dollar", ParentKey = null, Order = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu2, Key = "transaksi-stpb", Label = "SPTB", Icon = "file-text", ParentKey = "transaksi", Order = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu3, Key = "anggaran", Label = "Anggaran", Icon = "file-done", ParentKey = null, Order = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu4, Key = "anggaran-list", Label = "Daftar Anggaran", Icon = "unordered-list", ParentKey = "anggaran", Order = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu5, Key = "monitoring", Label = "Monitoring", Icon = "bar-chart", ParentKey = null, Order = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu6, Key = "master-data", Label = "Master Data", Icon = "database", ParentKey = null, Order = 4, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu7, Key = "master-ppkbendahara", Label = "PPK/Bendahara", Icon = "team", ParentKey = "master-data", Order = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu8, Key = "administration", Label = "Administration", Icon = "setting", ParentKey = null, Order = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu9, Key = "admin-users", Label = "User Management", Icon = "user", ParentKey = "administration", Order = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Menu { Id = menu10, Key = "admin-roles", Label = "Role Management", Icon = "safety", ParentKey = "administration", Order = 2, IsActive = true, CreatedAt = DateTime.UtcNow }
        );

        // Note: RoleMenuPermission seeding akan dilakukan via API endpoint seed-admin-permissions
        // karena MenuId adalah foreign key yang harus match dengan Menu.Id dari database
    }
}
