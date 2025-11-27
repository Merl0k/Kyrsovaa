using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Infrastructure;
using Domain.Models;

namespace Services
{
    public class StudentService
    {
        private readonly AppDbContext _db;

        public StudentService(AppDbContext db)
        {
            _db = db;
        }

        public void ListStudents()
        {
            var students = _db.Students.Include(s => s.Group).ToList();

            if (students.Count == 0)
            {
                Console.WriteLine("Нет студентов");
                return;
            }

            foreach (var s in students)
                Console.WriteLine($"{s.Id}: {s.FullName} (Группа: {s.Group?.Name})");
        }

        // Принимает fullName и groupNameOrId — если передан id (число строки), ищем по Id, иначе по имени.
        public void AddStudent(string fullName, string groupNameOrId)
        {
            Group? group = null;

            // Сначала ищем по имени
            group = _db.Groups.FirstOrDefault(g => g.Name == groupNameOrId);

            // Если не нашли по имени и ввод — число, пробуем поиск по ID
            if (group == null && int.TryParse(groupNameOrId, out int id))
                group = _db.Groups.FirstOrDefault(g => g.Id == id);

            if (group is null)
            {
                Console.WriteLine("Группа не найдена!");
                return;
            }

            _db.Students.Add(new Student
            {
                FullName = fullName,
                GroupId = group.Id
            });
            _db.SaveChanges();

            Console.WriteLine("Студент добавлен!");
        }
        public Student? GetStudentById(int id)
        {
            return _db.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);
        }

    }
}
