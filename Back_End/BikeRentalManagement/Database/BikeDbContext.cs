using System;
using BikeRentalManagement.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalManagement.Database;

public class BikeDbContext : DbContext
{
    public BikeDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User>Users{get;set;}
    public DbSet<Bike>Bikes{get;set;}
    public DbSet<BikeUnit>BikeUnits{get;set;}
    public DbSet<BikeImages>BikeImages{get;set;}
    public DbSet<Brand>Brands{get;set;}
    public DbSet<Model>Models{get;set;}
    public DbSet<Email>Emails{get;set;}
    public DbSet<Notification>Notifications {get;set;}
    public DbSet<RentalRequest>RentalRequests{get;set;}
    public DbSet<Entrylog>Entries{get;set;}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

    modelBuilder.Entity<User>()
        .Property(u=>u.Status)
        .HasConversion<string>();

        
    modelBuilder.Entity<Email>()
    .Property(u=>u.EmailType)
    .HasConversion<string>();

    modelBuilder.Entity<RentalRequest>()
    .Property(u=>u.Status)
    .HasConversion<string>();

    modelBuilder.Entity<User>()
        .HasMany(u => u.RentalRequests)
        .WithOne(r => r.User)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);  

    modelBuilder.Entity<User>()
        .HasMany(u => u.Notifications)
        .WithOne(n => n.User)
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<User>()
        .HasMany(u => u.Entrylogs)
        .WithOne(e => e.User)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Restrict);  

    modelBuilder.Entity<Bike>()
        .HasMany(b => b.BikeUnits)
        .WithOne(bu => bu.Bike)
        .HasForeignKey(bu => bu.BikeId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Bike>()
        .HasMany(b => b.RentalRequests)
        .WithOne(r => r.Bike)
        .HasForeignKey(r => r.BikeId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<BikeUnit>()
        .HasMany(bu => bu.bikeImages)
        .WithOne(bi => bi.BikeUnit)
        .HasForeignKey(bi => bi.UnitId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Email>()
        .HasMany(e => e.Notifications)
        .WithOne(n => n.Email)
        .HasForeignKey(n => n.EmailId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Notification>()
        .HasOne(n => n.Email)
        .WithMany(e => e.Notifications)
        .HasForeignKey(n => n.EmailId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Notification>()
        .HasOne(n => n.User)
        .WithMany(u => u.Notifications)
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<RentalRequest>()
        .HasOne(r => r.User)
        .WithMany(u => u.RentalRequests)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<RentalRequest>()
        .HasOne(r => r.Bike)
        .WithMany(b => b.RentalRequests)
        .HasForeignKey(r => r.BikeId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Entrylog>()
        .HasOne(e => e.User)
        .WithMany(u => u.Entrylogs)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Restrict);  
   
    modelBuilder.Entity<Bike>()
        .HasOne(b => b.Model)  
        .WithMany(m => m.Bikes)
        .HasForeignKey(b => b.ModelId)
        .OnDelete(DeleteBehavior.Restrict);  

    modelBuilder.Entity<Model>()
    .HasOne(m => m.Brand)
    .WithMany(b => b.Models)
    .HasForeignKey(m => m.BrandId)
    .OnDelete(DeleteBehavior.Restrict);

    base.OnModelCreating(modelBuilder);
}

   
}
