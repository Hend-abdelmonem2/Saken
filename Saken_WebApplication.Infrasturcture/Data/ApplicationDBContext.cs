using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Models.Guid;
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
        public DbSet<Agent> Agents { get; set; }
        public DbSet<CommissionTracking> commissionTrackings { get; set; }
        public DbSet<SavedHousing> SavedHousing { get; set; }
        public DbSet<HousingPhoto> housingPhotos { get; set; }
        public DbSet<InspectionSlot> InspectionSlots { get; set; }
        public DbSet<InspectionRequest> inspectionRequests { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<HousingOffer> housingOffer { get; set; }
        public DbSet<Contact> contacts { get; set; }
    


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Review
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
                .IsRequired(false);

            // Reservation -> Housing (خليها Cascade الوحيدة)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Housing)
                .WithMany(h => h.Reservations)
                .HasForeignKey(r => r.HousingId)
                .OnDelete(DeleteBehavior.Cascade);

            // User Email index
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(false);

            // Housing enums as string
            modelBuilder.Entity<Housing>(entity =>
            {
                entity.Property(h => h.HousingType).HasConversion<string>();
                entity.Property(h => h.FurnishingStatus).HasConversion<string>();
                entity.Property(h => h.TargetTenantType).HasConversion<string>();
                entity.Property(h => h.RentdurationUnit).HasConversion<string>();
                entity.Property(h => h.RentalType).HasConversion<string>();
            });

            // UserPreferences enums as string
            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.Property(e => e.PreferredPropertyType).HasConversion<string>();
                entity.Property(e => e.PreferredFurnishing).HasConversion<string>();
                entity.Property(e => e.PreferredDuration).HasConversion<string>();
                entity.Property(e => e.PreferredTenantType).HasConversion<string>();
                entity.Property(e => e.PreferredTargetCustomer).HasConversion<string>();
                entity.Property(e => e.PreferredTenantType).HasConversion<string>();
            });

            // Reservation -> Tenant
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation -> Landlord
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Landlord)
                .WithMany()
                .HasForeignKey(r => r.LandlordId)
                .OnDelete(DeleteBehavior.Restrict);

            // SavedHousing -> Housing
            modelBuilder.Entity<SavedHousing>()
                .HasOne(s => s.Housing)
                .WithMany(h => h.SavedByUsers)
                .HasForeignKey(s => s.HousingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contact -> OwnerUser
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.OwnerUser)
                .WithMany()
                .HasForeignKey(c => c.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Contact -> ContactUser
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.ContactUser)
                .WithMany()
                .HasForeignKey(c => c.ContactUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message -> Sender
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message -> Receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // CommissionTracking -> Housing (Restrict عشان تمنع multiple cascade)
            modelBuilder.Entity<CommissionTracking>()
                .HasOne(c => c.Housing)
                .WithMany(h => h.CommissionTrackings)
                .HasForeignKey(c => c.HousingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CommissionTracking>()
            .HasOne(c => c.Agent)
             .WithMany()
             .HasForeignKey(c => c.AgentId)
              .OnDelete(DeleteBehavior.NoAction);

        

            modelBuilder.Entity<Review>()
                .Property(r => r.ReviewType)
                .HasConversion<string>(); 
        
    }

    }
}
