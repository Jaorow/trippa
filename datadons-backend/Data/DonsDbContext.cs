using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Data
{
    public class DonsDbContext : DbContext
    {
        public DonsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<GPS> GPS { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - IncomingReviews (one-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.IncomingReviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - OutgoingReviews (one-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.OutgoingReviews)
                .WithOne()
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Driver (one-to-one)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Driver)
                .WithOne(d => d.User)
                .HasForeignKey<Driver>(d => d.UserId)
                .IsRequired(false) // Optional: User is not required to have a driver
                .OnDelete(DeleteBehavior.Cascade);

            // Car - Make (enum as string)
            modelBuilder.Entity<Car>()
                .Property(c => c.Make)
                .HasConversion<string>();

            // Review - User (many-to-one)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.IncomingReviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review - Reviewer (many-to-one)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trip - CurrentRiders (many-to-many)
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.CurrentRiders)
                .WithMany(u => u.RiddenTrips)
                .UsingEntity(j => j.ToTable("TripRiders"));


            // Trip - DriverId (many-to-one)
            modelBuilder.Entity<Trip>()
                .HasOne<Driver>()
                .WithMany()
                .HasForeignKey(t => t.DriverID)
                .HasPrincipalKey(d => d.UserId);

            // User - DrivenTrips (one-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.DrivenTrips)
                .WithOne()
                .HasForeignKey(t => t.DriverID);

            // GPS (owned by Trip)
            modelBuilder.Entity<GPS>()
                .HasKey(g => g.Id);

            // Preference - Trip (many-to-one)
            modelBuilder.Entity<Preference>()
                .HasOne(p => p.Trip)
                .WithMany(t => t.Preferences)
                .HasForeignKey(p => p.TripId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection here
            // optionsBuilder.UseSqlServer("YourConnectionString");
            optionsBuilder.UseSqlite("Data Source=DonsDb.sqlite");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
