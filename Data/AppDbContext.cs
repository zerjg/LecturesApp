using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LecturesApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LecturesApp.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLecture>()
                .HasKey(k => new { k.UserID, k.LectureID });

            modelBuilder.Entity<UserLecture>()
                .HasOne(e => e.User)
                .WithMany(m => m.RegisteredOnLecturesLink)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<UserLecture>()
                .HasOne(e => e.Lecture)
                .WithMany(l => l.RegisteredMembersLink)
                .HasForeignKey(e => e.LectureID);

            modelBuilder.Entity<Lecture>()
                .HasOne(l => l.HostUser)
                .WithMany(h => h.HostedLectures)
                .HasForeignKey(l => l.HostUserID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
