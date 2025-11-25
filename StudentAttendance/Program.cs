using System;
using Infrastructure;
using Services;
using Domain.Models;
using Data;

class Program
{
    static void Main()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();
        SeedData.Initialize(db);

        var service = new AttendanceService(db);

        while (true)
        {
            Console.WriteLine("\n--- Меню учёта посещаемости ---");
            Console.WriteLine("1. Показать всех студентов");
            Console.WriteLine("2. Добавить студента");
            Console.WriteLine("3. Добавить группу");
            Console.WriteLine("4. Добавить занятие");
            Console.WriteLine("5. Отметить посещение");
            Console.WriteLine("6. Отчёт: % посещаемости по студентам");
            Console.WriteLine("7. Отчёт: Худшие студенты");
            Console.WriteLine("8. Отчёт по группам");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите: ");
            var key = Console.ReadLine();

            switch (key)
            {
                case "1":
                    foreach (var s in service.GetAllStudents())
                        Console.WriteLine($"{s.Id}: {s.FullName} (GroupId={s.GroupId})");
                    break;
                case "2":
                    Console.Write("ФИО: "); var name = Console.ReadLine();
                    Console.Write("GroupId: "); var gid = int.Parse(Console.ReadLine() ?? "0");
                    service.AddStudent(new Student { FullName = name ?? "", GroupId = gid });
                    Console.WriteLine("Студент добавлен.");
                    break;
                case "3":
                    Console.Write("Название группы: "); var gname = Console.ReadLine();
                    service.AddGroup(new Group { Name = gname ?? "" });
                    Console.WriteLine("Группа добавлена.");
                    break;
                case "4":
                    Console.Write("Предмет: "); var subj = Console.ReadLine();
                    Console.Write("Дата (yyyy-MM-dd): "); var dstr = Console.ReadLine();
                    Console.Write("GroupId: "); var gid2 = int.Parse(Console.ReadLine() ?? "0");
                    if (DateTime.TryParse(dstr, out var date))
                    {
                        service.AddSession(new ClassSession { Subject = subj ?? "", Date = date, GroupId = gid2 });
                        Console.WriteLine("Занятие добавлено.");
                    }
                    else Console.WriteLine("Неверная дата.");
                    break;
                case "5":
                    Console.Write("StudentId: "); var sid = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("ClassSessionId: "); var csid = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("IsPresent (y/n): "); var pres = Console.ReadLine();
                    bool isPresent = pres?.ToLower().StartsWith("y") ?? false;
                    service.AddAttendance(new Attendance { StudentId = sid, ClassSessionId = csid, IsPresent = isPresent });
                    Console.WriteLine("Отметка добавлена.");
                    break;
                case "6":
                    var list = service.GetAttendancePercentPerStudent();
                    foreach (var item in list)
                        Console.WriteLine($"{item.student.Id}: {item.student.FullName} — {item.percent:F1}%");
                    break;
                case "7":
                    var worst = service.GetWorstStudents(5);
                    foreach (var w in worst)
                        Console.WriteLine($"{w.student.Id}: {w.student.FullName} — {w.percent:F1}%");
                    break;
                case "8":
                    var report = service.GetReportByGroups();
                    foreach (var r in report)
                    {
                        Console.WriteLine($"\nГруппа: {r.group.Name} — средняя посещаемость: {r.avgPercent:F1}%");
                        foreach (var m in r.members)
                            Console.WriteLine($"  {m.student.Id}: {m.student.FullName} — {m.percent:F1}%");
                    }
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }
}
