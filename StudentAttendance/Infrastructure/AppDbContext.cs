using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    /// <summary>
    /// EF Core DbContext для приложения.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<ClassSession> ClassSessions { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SQLite-файл в рабочей папке
            optionsBuilder.UseSqlite("Data Source=student_attendance.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Простые конфигурации можно добавить здесь при необходимости
            base.OnModelCreating(modelBuilder);
        }
    }
}
