using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Data
{
    public class ApplicationDBContext : IdentityDbContext<User> // Fixed casing for consistency
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Housing> houses { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<SavedHousing> SavedHousing { get; set; }
        public DbSet<HousingPhoto> housingPhotos { get; set; }
        public DbSet<InspectionSlot> InspectionSlots { get; set; }
        public DbSet<InspectionRequest> inspectionRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);  // تجنب الحذف التتابعي

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);  // نفس الشيء هنا



            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsSubmitted)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedUser)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); // السماح بأن يكون null

            modelBuilder.Entity<Reservation>()
        .HasOne(r => r.Housing)
        .WithMany(h => h.Reservations)
        .HasForeignKey(r => r.HousingId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
              .HasIndex(u => u.Email)
              .IsUnique(false);


            modelBuilder.Entity<Housing>(entity =>
            {
                entity.Property(h => h.HousingType)
                      .HasConversion<string>();

                entity.Property(h => h.FurnishingStatus)
                      .HasConversion<string>();

                entity.Property(h => h.TargetTenantType)
                      .HasConversion<string>();

                entity.Property(h => h.RentdurationUnit)
                      .HasConversion<string>();

                entity.Property(h => h.RentalType)
                      .HasConversion<string>();
            });
            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.Property(e => e.PreferredPropertyType).HasConversion<string>();
                entity.Property(e => e.PreferredFurnishing).HasConversion<string>();
                entity.Property(e => e.PreferredDuration).HasConversion<string>();
                entity.Property(e => e.PreferredTenantType).HasConversion<string>();
                entity.Property(e => e.PreferredTargetCustomer).HasConversion<string>();
                entity.Property(e => e.PreferredTenantType).HasConversion<string>();
            });

            modelBuilder.Entity<Reservation>()
       .HasOne(r => r.Tenant)
       .WithMany()
       .HasForeignKey(r => r.UserId)
       .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع الـ Landlord (صاحب السكن)
            
            modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Landlord)
            .WithMany()
            .HasForeignKey(r => r.LandlordId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedHousing>()
    .HasOne(s => s.Housing)
    .WithMany(h => h.SavedByUsers)
    .HasForeignKey(s => s.HousingId)
    .OnDelete(DeleteBehavior.Restrict);

        }
    
    }
}
