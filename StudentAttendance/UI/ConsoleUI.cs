using System;
using Services;
using Domain.Models;

namespace UI
{
    public class ConsoleUI
    {
        private readonly StudentService _studentService;
        private readonly AttendanceService _attendanceService;
        private readonly GroupService _groupService;

        public ConsoleUI(StudentService studentService,
                         AttendanceService attendanceService,
                         GroupService groupService)
        {
            _studentService = studentService;
            _attendanceService = attendanceService; // запасной вариант: если в твоём коде инъекция по другому, просто используй имена полей
            _groupService = groupService;
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
                Console.WriteLine("6 - Список групп");
                Console.WriteLine("7 - Добавить группу");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _studentService.ListStudents();
                        break;

                    case "2":
                        Console.Write("Имя студента: ");
                        string fullName = Console.ReadLine()!;
                        Console.Write("Группа (введи имя группы ИЛИ её ID): ");
                        string groupInput = Console.ReadLine()!;
                        // StudentService.AddStudent умеет принимать и имя, и id в виде строки
                        _studentService.AddStudent(fullName, groupInput);
                        break;

                    case "3":
                        Console.Write("ID студента: ");
                        if (!int.TryParse(Console.ReadLine(), out int sid))
                        {
                            Console.WriteLine("Неверный ID студента.");
                            break;
                        }

                        var student = _studentService.GetStudentById(sid);
                        if (student == null)
                        {
                            Console.WriteLine("Студент не найден.");
                            break;
                        }

                        Console.Write("Тема занятия: ");
                        string topic = Console.ReadLine()!;
                        Console.Write("Присутствовал? (y/n): ");
                        bool present = Console.ReadLine()!.Trim().ToLower() == "y";

                        // Передаем groupId студента
                        _attendanceService.MarkAttendanceByTopic(topic, sid, present, student.GroupId);
                        break;




                    case "4":
                        _attendanceService.ShowWorstAttendance();
                        break;

                    case "5":
                        var report = _attendanceService.ReportByGroup();

                        Console.WriteLine("\nОтчёт по группам:");

                        string filePath = "GroupReport.txt"; // имя файла
                        using (var writer = new StreamWriter(filePath, false)) // false = перезаписываем файл
                        {
                            foreach (var (group, avg, members) in report)
                            {
                                string header = $"\nГруппа: {group.Name}\nСредняя посещаемость: {avg:F2}%\nСтуденты:";
                                Console.WriteLine(header);
                                writer.WriteLine(header);

                                foreach (var (stud, percent) in members)
                                {
                                    string line = $"  {stud.FullName} — {percent:F2}%";
                                    Console.WriteLine(line);
                                    writer.WriteLine(line);
                                }
                            }
                        }

                        Console.WriteLine($"\nОтчёт сохранён в файл '{filePath}'");
                        break;


                    case "6":
                        _groupService.ListGroups();
                        break;

                    case "7":
                        Console.Write("Название группы: ");
                        string groupName = Console.ReadLine()!;
                        _groupService.AddGroup(groupName);
                        break;

                    case "0": return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}
