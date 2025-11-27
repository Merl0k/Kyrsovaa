using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    /// <summary>
    /// EF Core DbContext
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<ClassSession> ClassSessions => Set<ClassSession>();
        public DbSet<Attendance> Attendances => Set<Attendance>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Используем SQLite и файл student_attendance.db
            optionsBuilder.UseSqlite("Data Source=student_attendance.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Group -> Students, Group -> Sessions
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(s => s.GroupId);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Sessions)
                .WithOne(s => s.Group)
                .HasForeignKey(s => s.GroupId);

            // ClassSession -> Attendances
            modelBuilder.Entity<ClassSession>()
                .HasMany(cs => cs.Attendances)
                .WithOne(a => a.ClassSession)
                .HasForeignKey(a => a.ClassSessionId);

            // Student -> Attendances
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Attendances)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentId);

            // (необязательно) индекс для ускорения поиска посещаемости по паре Student+Session
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.StudentId, a.ClassSessionId })
                .IsUnique(false);
        }
    }
}
