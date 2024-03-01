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

			modelBuilder.Entity<User>().ToTable("users");
			modelBuilder.Entity<Student>().ToTable("students");
			modelBuilder.Entity<Teacher>().ToTable("teacher");
			modelBuilder.Entity<Review>().ToTable("review");

			modelBuilder.Entity<Student>()
				.HasKey(s => s.MSSV);

			modelBuilder.Entity<Teacher>()
				.HasKey(t => t.MSGV);

			modelBuilder.Entity<Review>()
				.HasKey(r => r.Id);

			modelBuilder.Entity<Teacher>()
				.HasMany(t => t.Students)
				.WithOne(s => s.Teacher)
				.HasForeignKey(s => s.MSGV)
				.OnDelete(DeleteBehavior.Restrict); 

			modelBuilder.Entity<Student>()
				.HasOne(s => s.Teacher)
				.WithMany(t => t.Students)
				.HasForeignKey(s => s.MSGV)
				.OnDelete(DeleteBehavior.Restrict); 
		}

	}
}