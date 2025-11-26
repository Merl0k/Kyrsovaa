using System;
using Services;
using Domain.Models;
using System.Linq;

namespace UI
{
    public class ConsoleUI
    {
        private readonly AttendanceService _service;

        public ConsoleUI(AttendanceService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1 - Список студентов");
                Console.WriteLine("2 - Добавить студента");
                Console.WriteLine("3 - Отметить посещаемость");
                Console.WriteLine("4 - Худшие по посещаемости");
                Console.WriteLine("5 - Отчет по группам");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");
                var cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "1":
                        var students = _service.GetAllStudents();
                        foreach (var s in students)
                            Console.WriteLine($"{s.Id}: {s.FullName} (Группа: {s.Group?.Name ?? "N/A"})");
                        break;
                    case "2":
                        Console.Write("ФИО студента: ");
                        var name = Console.ReadLine() ?? "";
                        Console.Write("Id группы: ");
                        if (!int.TryParse(Console.ReadLine(), out var gid))
                        {
                            Console.WriteLine("Неверный id группы.");
                            break;
                        }
                        var st = new Student { FullName = name, GroupId = gid };
                        _service.AddStudent(st);
                        Console.WriteLine("Студент добавлен.");
                        break;
                    case "3":
                        Console.Write("Id занятия: ");
                        if (!int.TryParse(Console.ReadLine(), out var sid)) { Console.WriteLine("Неверно"); break; }
                        Console.Write("Id студента: ");
                        if (!int.TryParse(Console.ReadLine(), out var studId)) { Console.WriteLine("Неверно"); break; }
                        Console.Write("Присутствовал? (y/n): ");
                        var ans = Console.ReadLine();
                        var present = ans?.ToLower() == "y";
                        _service.MarkAttendance(sid, studId, present);
                        Console.WriteLine("Сохранено.");
                        break;
                    case "4":
                        var worst = _service.GetWorstStudents(10);
                        Console.WriteLine("Худшие по посещаемости:");
                        foreach (var w in worst)
                            Console.WriteLine($"{w.student.Id}: {w.student.FullName} — {w.percent:F1}%");
                        break;
                    case "5":
                        var rep = _service.ReportByGroup();
                        foreach (var r in rep)
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
}
