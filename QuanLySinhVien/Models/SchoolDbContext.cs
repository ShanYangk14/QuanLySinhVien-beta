using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models;
using System.Security.Cryptography.X509Certificates;

namespace QuanLySinhVien.Data
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Individual> individuals { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeacherEvaluation> Teachers { get; set; }
        public DbSet<Grade> Grades { get; set; }
        
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<TeacherEvaluation>().ToTable("TeacherEvaluations");
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<Grade>().ToTable("Grades");

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Individual)
                .WithOne(i => i.Student)
                .HasForeignKey<Individual>(i => i.StudentId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Team)
                .WithOne(t => t.Student)
                .HasForeignKey<Team>(t => t.StudentId);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.TeacherEvaluation)
                .WithOne(te => te.Student)
                .HasForeignKey<TeacherEvaluation>(te => te.StudentId);

            modelBuilder.Entity<Grade>()
                .Property(g => g.MaxScore)
                .HasDefaultValue(10);

            base.OnModelCreating(modelBuilder);
        }
    }
}
