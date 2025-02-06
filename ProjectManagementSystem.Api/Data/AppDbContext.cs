using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
namespace ProjectManagementSystem.Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<TempAuthCode> TempAuthCodes { get; set; }
    public DbSet<RoleFeature> RoleFeatures { get; set; }

    public DbSet<ProjectUserRoles> ProjectUserRoles { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
