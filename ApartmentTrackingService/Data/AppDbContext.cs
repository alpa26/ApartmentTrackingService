using ApartmentTrackingService.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;

namespace ApartmentTrackingService.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public AppDbContext()
    {
    }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Apartment>().ToTable("apartments");

        modelBuilder.Entity<User>()
        .HasMany(e => e.Apartments)
        .WithMany(e => e.Users)
        .UsingEntity(
            "userapartment",
            r => r.HasOne(typeof(Apartment)).WithMany().HasForeignKey("ApartmentId"),
            l => l.HasOne(typeof(User)).WithMany().HasForeignKey("UserId"),
            j => j.HasKey("UserId", "ApartmentId"));
    }
}
