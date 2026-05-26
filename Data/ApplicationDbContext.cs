using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<ExpenseClaim> ExpenseClaims => Set<ExpenseClaim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(user => user.UserName).IsUnique();
            entity.Property(user => user.UserName).HasMaxLength(50).IsRequired();
            entity.Property(user => user.DisplayName).HasMaxLength(80).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<ExpenseClaim>(entity =>
        {
            entity.HasIndex(claim => claim.ClaimNo).IsUnique();
            entity.Property(claim => claim.ClaimNo).HasMaxLength(20).IsRequired();
            entity.Property(claim => claim.Category).HasMaxLength(50).IsRequired();
            entity.Property(claim => claim.Description).HasMaxLength(500).IsRequired();
            entity.Property(claim => claim.Status).HasConversion<string>().HasMaxLength(30);
            entity.Property(claim => claim.Amount).HasPrecision(12, 2);
            entity.HasOne(claim => claim.Applicant)
                .WithMany()
                .HasForeignKey(claim => claim.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
