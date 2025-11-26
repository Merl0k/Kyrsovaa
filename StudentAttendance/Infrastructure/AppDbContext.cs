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
            // »спользуем SQLite Ч файл в корне проекта student_attendance.db
            optionsBuilder.UseSqlite("Data Source=student_attendance.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasMany(g => g.Students).WithOne(s => s.Group).HasForeignKey(s => s.GroupId);
            modelBuilder.Entity<Group>().HasMany(g => g.Sessions).WithOne(s => s.Group).HasForeignKey(s => s.GroupId);

            modelBuilder.Entity<ClassSession>().HasMany(cs => cs.Attendances).WithOne(a => a.ClassSession).HasForeignKey(a => a.ClassSessionId);
            modelBuilder.Entity<Student>().HasMany(s => s.Attendances).WithOne(a => a.Student).HasForeignKey(a => a.StudentId);

            // Composite unique constraint could be added if needed (student + session)
            modelBuilder.Entity<Attendance>().HasIndex(a => new { a.StudentId, a.ClassSessionId }).IsUnique(false);
        }
    }
}
