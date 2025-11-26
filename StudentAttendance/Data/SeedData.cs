using Domain.Models;
using System;
using System.Linq;

namespace Data
{
    public static class SeedData
    {
        public static void Initialize(Infrastructure.AppDbContext db)
        {
            if (db.Groups.Any()) return;

            var g1 = new Group { Name = "CS-101" };
            var g2 = new Group { Name = "CS-102" };
            db.Groups.AddRange(g1, g2);
            db.SaveChanges();

            var s1 = new Student { FullName = "Ivan Petrov", GroupId = g1.Id };
            var s2 = new Student { FullName = "Olga Ivanova", GroupId = g1.Id };
            var s3 = new Student { FullName = "Petr Sidorov", GroupId = g2.Id };
            db.Students.AddRange(s1, s2, s3);
            db.SaveChanges();

            var d1 = DateTime.Today.AddDays(-10);
            var d2 = DateTime.Today.AddDays(-7);
            var ses1 = new ClassSession { Date = d1, Topic = "Intro", GroupId = g1.Id };
            var ses2 = new ClassSession { Date = d2, Topic = "Advanced", GroupId = g1.Id };
            db.ClassSessions.AddRange(ses1, ses2);
            db.SaveChanges();

            db.Attendances.AddRange(
                new Attendance { StudentId = s1.Id, ClassSessionId = ses1.Id, IsPresent = true },
                new Attendance { StudentId = s2.Id, ClassSessionId = ses1.Id, IsPresent = false },
                new Attendance { StudentId = s1.Id, ClassSessionId = ses2.Id, IsPresent = true },
                new Attendance { StudentId = s2.Id, ClassSessionId = ses2.Id, IsPresent = true }
            );
            db.SaveChanges();
        }
    }
}
