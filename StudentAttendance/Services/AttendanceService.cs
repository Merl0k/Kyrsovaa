using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    /// <summary>
    /// Сервис с бизнес-логикой и отчётами.
    /// </summary>
    public class AttendanceService
    {
        private readonly AppDbContext _db;
        private readonly GenericRepository<Student> _studentRepo;
        private readonly GenericRepository<Attendance> _attendanceRepo;
        private readonly GenericRepository<ClassSession> _sessionRepo;
        private readonly GenericRepository<Group> _groupRepo;

        public AttendanceService(AppDbContext db)
        {
            _db = db;
            _studentRepo = new GenericRepository<Student>(_db);
            _attendanceRepo = new GenericRepository<Attendance>(_db);
            _sessionRepo = new GenericRepository<ClassSession>(_db);
            _groupRepo = new GenericRepository<Group>(_db);
        }

        /// <summary>
        /// Возвращает процент посещаемости каждого студента (0..100).
        /// </summary>
        public IEnumerable<(Student student, double percent)> GetAttendancePercentPerStudent()
        {
            var students = _db.Students.Include(s => s.Attendances).ThenInclude(a => a.ClassSession).Include(s => s.Group).ToList();

            var results = students.Select(s =>
            {
                var total = s.Attendances.Count;
                var present = s.Attendances.Count(a => a.IsPresent);
                double percent = total == 0 ? 0.0 : (present * 100.0) / total;
                return (student: s, percent);
            })
            .OrderByDescending(x => x.percent);

            return results;
        }

        /// <summary>
        /// Находит N студентов с худшей посещаемостью.
        /// </summary>
        public IEnumerable<(Student student, double percent)> GetWorstStudents(int topN = 5)
        {
            return GetAttendancePercentPerStudent().OrderBy(x => x.percent).Take(topN);
        }

        /// <summary>
        /// Отчёт по группам: средний процент посещаемости группы и список студентов.
        /// </summary>
        public IEnumerable<(Group group, double avgPercent, List<(Student student, double percent)> members)> GetReportByGroups()
        {
            var groups = _db.Groups.Include(g => g.Students).ThenInclude(s => s.Attendances).ToList();
            var report = new List<(Group, double, List<(Student, double)>)>();

            foreach (var g in groups)
            {
                var members = g.Students.Select(s =>
                {
                    var total = s.Attendances.Count;
                    var present = s.Attendances.Count(a => a.IsPresent);
                    double p = total == 0 ? 0.0 : (present * 100.0) / total;
                    return (s, p);
                }).ToList();

                double avg = members.Count == 0 ? 0.0 : members.Average(m => m.Item2);
                report.Add((g, avg, members));
            }

            return report;
        }

        // Дополнительно: CRUD-обёртки, помогающие UI
        public void AddStudent(Student s) { _studentRepo.Add(s); _studentRepo.SaveChanges(); }
        public void AddGroup(Group g) { _groupRepo.Add(g); _groupRepo.SaveChanges(); }
        public void AddSession(ClassSession cs) { _sessionRepo.Add(cs); _sessionRepo.SaveChanges(); }
        public void AddAttendance(Attendance a) { _attendanceRepo.Add(a); _attendanceRepo.SaveChanges(); }

        public IEnumerable<Student> GetAllStudents() => _studentRepo.GetAll();
        public IEnumerable<Group> GetAllGroups() => _groupRepo.GetAll();
        public IEnumerable<ClassSession> GetAllSessions() => _sessionRepo.GetAll();
    }
}
