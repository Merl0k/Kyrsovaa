using Domain.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    /// <summary>
    /// —ервис по работе с посещаемостью и студентами
    /// </summary>
    public class AttendanceService
    {
        private readonly AppDbContext _db;
        public AttendanceService(AppDbContext db)
        {
            _db = db;
        }

        // CRUD Students
        public IEnumerable<Student> GetAllStudents() =>
            _db.Students.Include(s => s.Group).ToList();

        public Student? GetStudent(int id) =>
            _db.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);

        public void AddStudent(Student s)
        {
            _db.Students.Add(s);
            _db.SaveChanges();
        }

        public void UpdateStudent(Student s)
        {
            _db.Students.Update(s);
            _db.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            var s = _db.Students.Find(id);
            if (s != null)
            {
                _db.Students.Remove(s);
                _db.SaveChanges();
            }
        }

        // Sessions
        public IEnumerable<ClassSession> GetSessionsForGroup(int groupId) =>
            _db.ClassSessions.Where(cs => cs.GroupId == groupId).Include(cs => cs.Attendances).ToList();

        public void AddSession(ClassSession cs)
        {
            _db.ClassSessions.Add(cs);
            _db.SaveChanges();
        }

        // Attendance marking
        public void MarkAttendance(int sessionId, int studentId, bool present)
        {
            var att = _db.Attendances.FirstOrDefault(a => a.ClassSessionId == sessionId && a.StudentId == studentId);
            if (att == null)
            {
                att = new Attendance { ClassSessionId = sessionId, StudentId = studentId, IsPresent = present };
                _db.Attendances.Add(att);
            }
            else
            {
                att.IsPresent = present;
                _db.Attendances.Update(att);
            }
            _db.SaveChanges();
        }

        // Reports
        public double GetStudentAttendancePercent(int studentId)
        {
            var total = _db.Attendances.Count(a => a.StudentId == studentId);
            if (total == 0) return 0.0;
            var present = _db.Attendances.Count(a => a.StudentId == studentId && a.IsPresent);
            return (double)present / total * 100.0;
        }

        public IEnumerable<(Student student, double percent)> GetWorstStudents(int top = 5)
        {
            var students = _db.Students.Include(s => s.Attendances).ToList();
            var q = students.Select(s =>
            {
                var total = s.Attendances?.Count() ?? 0;
                var present = s.Attendances?.Count(a => a.IsPresent) ?? 0;
                var percent = total == 0 ? 0.0 : (double)present / total * 100.0;
                return (student: s, percent);
            })
            .OrderBy(x => x.percent)
            .Take(top);
            return q;
        }

        public IEnumerable<(Group group, double avgPercent, IEnumerable<(Student student, double percent)> members)> ReportByGroup()
        {
            var groups = _db.Groups.Include(g => g.Students).ThenInclude(s => s.Attendances).ToList();
            foreach (var g in groups)
            {
                var members = g.Students?.Select(s =>
                {
                    var total = s.Attendances?.Count() ?? 0;
                    var present = s.Attendances?.Count(a => a.IsPresent) ?? 0;
                    var percent = total == 0 ? 0.0 : (double)present / total * 100.0;
                    return (student: s, percent);
                }) ?? Enumerable.Empty<(Student, double)>();

                var avg = members.Any() ? members.Average(m => m.percent) : 0.0;
                yield return (g, avg, members);
            }
        }
    }
}
