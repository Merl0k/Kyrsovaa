using System;
using System.Collections.Generic;
using Domain.Models;
using Infrastructure;

namespace Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext db)
        {
            if (db.Groups.Any()) return;

            var g1 = new Group { Name = "KN-21" };
            var g2 = new Group { Name = "IS-20" };

            db.Groups.AddRange(g1, g2);
            db.SaveChanges();

            var s1 = new Student { FullName = "Иванов Иван", GroupId = g1.Id };
            var s2 = new Student { FullName = "Петров Пётр", GroupId = g1.Id };
            var s3 = new Student { FullName = "Сидорова Ольга", GroupId = g2.Id };

            db.Students.AddRange(s1, s2, s3);
            db.SaveChanges();

            var cs1 = new ClassSession { Date = DateTime.Today.AddDays(-7), Subject = "Математика", GroupId = g1.Id };
            var cs2 = new ClassSession { Date = DateTime.Today.AddDays(-3), Subject = "Физика", GroupId = g1.Id };
            var cs3 = new ClassSession { Date = DateTime.Today.AddDays(-5), Subject = "Информатика", GroupId = g2.Id };

            db.ClassSessions.AddRange(cs1, cs2, cs3);
            db.SaveChanges();

            var attendances = new List<Attendance>
            {
                new Attendance{ StudentId = s1.Id, ClassSessionId = cs1.Id, IsPresent = true },
                new Attendance{ StudentId = s2.Id, ClassSessionId = cs1.Id, IsPresent = false },
                new Attendance{ StudentId = s1.Id, ClassSessionId = cs2.Id, IsPresent = true },
                new Attendance{ StudentId = s2.Id, ClassSessionId = cs2.Id, IsPresent = false },
                new Attendance{ StudentId = s3.Id, ClassSessionId = cs3.Id, IsPresent = true },
            };

            db.Attendances.AddRange(attendances);
            db.SaveChanges();
        }
    }
}
