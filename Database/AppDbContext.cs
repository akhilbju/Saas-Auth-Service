using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Project> Projects { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .Property(u => u.Id)
                    .ValueGeneratedOnAdd();

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId);
            entity.Property(e => e.TokenId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Project>()
            .HasMany(p => p.TeamMembers)
            .WithMany(u => u.Projects);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.CreatedBy)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
