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



        }
    
    }
}
