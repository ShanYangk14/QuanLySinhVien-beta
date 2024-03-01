using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using System.Security.Cryptography.X509Certificates;

namespace QuanLySinhVien.Data
{
    public class SchoolDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasKey(s => s.MSSV);

            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.MSGV);

            modelBuilder.Entity<Review>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Manager>()
                .HasKey(m => m.AdminId);

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Students)
                .WithOne(s => s.Teacher)
                .HasForeignKey(s => s.MSGV)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.MSSV);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Teacher)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.MSGV);

            modelBuilder.Entity<User>()
                 .HasOne(u => u.Student)  
                 .WithMany(s => s.Users)
                 .HasForeignKey(u => u.idUser)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Teacher)  
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.idUser)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Manager)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.idUser)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    }