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
                Console.WriteLine("1 - Список студентів");
                Console.WriteLine("2 - Додати студента");
                Console.WriteLine("3 - Відзначити відвідуваність");
                Console.WriteLine("4 - Найгірші за відвідуваністю");
                Console.WriteLine("5 - Звіт по групах");
                Console.WriteLine("6 - Список груп");
                Console.WriteLine("7 - Додати групу");
                Console.WriteLine("8 - Редагувати студента");
                Console.WriteLine("9 - Видалити студента");
                Console.WriteLine("10 - Редагувати групу за назвою");
                Console.WriteLine("11 - Видалити групу");
                Console.WriteLine("0 - Вихід");
                Console.Write("Вибір: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _studentService.ListStudents();
                        break;

                    case "2":
                        Console.Write("Ім'я студента: ");
                        string fullName = Console.ReadLine()!;
                        Console.Write("Група: ");
                        string groupInput = Console.ReadLine()!;
                        _studentService.AddStudent(fullName, groupInput);
                        break;

                    case "3":
                        Console.Write("ID студента: ");
                        if (!int.TryParse(Console.ReadLine(), out int sid))
                        {
                            Console.WriteLine("Неправильний ID студента.");
                            break;
                        }

                        var student = _studentService.GetStudentById(sid);
                        if (student == null)
                        {
                            Console.WriteLine("Студент не знайдений.");
                            break;
                        }

                        Console.Write("Заняття: ");
                        string topic = Console.ReadLine()!;
                        Console.Write("Був присутній? (y/n): ");
                        bool present = Console.ReadLine()!.Trim().ToLower() == "y";

                        // Передаем groupId студента
                        _attendanceService.MarkAttendanceByTopic(topic, sid, present, student.GroupId);
                        break;




                    case "4":
                        _attendanceService.ShowWorstAttendance();
                        break;

                    case "5":
                        var report = _attendanceService.ReportByGroup();

                        Console.WriteLine("\nЗвіт по групах:");

                        string filePath = "GroupReport.txt"; // имя файла
                        using (var writer = new StreamWriter(filePath, false)) // false = перезаписываем файл
                        {
                            foreach (var (group, avg, members) in report)
                            {
                                string header = $"\nГрупа: {group.Name}\nСередня відвідуваність: {avg:F2}%\nСтуденти:";
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

                        Console.WriteLine($"\nЗвіт збережено у файл '{filePath}'");
                        break;


                    case "6":
                        _groupService.ListGroups();
                        break;

                    case "7":
                        Console.Write("Назва групи: ");
                        string groupName = Console.ReadLine()!;
                        _groupService.AddGroup(groupName);
                        break;

                    case "8": // Новый пункт "Редактировать студента"
                        Console.Write("ID студента для редагування: ");
                        int editId = int.Parse(Console.ReadLine()!);
                        Console.Write("Нове ім'я (Enter, щоб пропустити): ");
                        string? newName = Console.ReadLine();
                        Console.Write("Нова група (ім'я або ID, Enter, щоб пропустити): ");
                        string? newGroup = Console.ReadLine();
                        _studentService.EditStudent(editId, newName, newGroup);
                        break;

                    case "9": // Удалить студента
                        Console.Write("ID студента для видалення: ");
                        int delId = int.Parse(Console.ReadLine()!);
                        _studentService.DeleteStudent(delId);
                        break;

                    case "10": // Редактировать группу по имени
                        Console.Write("Введіть поточну назву групи: ");
                        string currentName = Console.ReadLine()!;
                        Console.Write("Введіть нову назву групи: ");
                        string newGroupName = Console.ReadLine()!; // <--- новое имя переменной
                        _groupService.EditGroupByName(currentName, newGroupName);
                        break;


                    case "11": // Удалить группу
                        Console.Write("ID групи для видалення: ");
                        int delGid = int.Parse(Console.ReadLine()!);
                        _groupService.DeleteGroup(delGid);
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
