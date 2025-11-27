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

        public StudentService(AppDbContext db) => _db = db;

        public void ListStudents()
        {
            var students = _db.Students.Include(s => s.Group).ToList();
            if (students.Count == 0) { Console.WriteLine("Нет студентов"); return; }

            foreach (var s in students)
                Console.WriteLine($"{s.Id}: {s.FullName} (Группа: {s.Group?.Name})");
        }

        public void AddStudent(string fullName, string groupNameOrId)
        {
            Group? group = null;

            // Ищем по имени группы
            group = _db.Groups.FirstOrDefault(g => g.Name == groupNameOrId);

            // Если не нашли по имени и введено число, пробуем поиск по ID
            if (group == null && int.TryParse(groupNameOrId, out int id))
                group = _db.Groups.FirstOrDefault(g => g.Id == id);

            if (group is null) { Console.WriteLine("Группа не найдена!"); return; }

            _db.Students.Add(new Student { FullName = fullName, GroupId = group.Id });
            _db.SaveChanges();
            Console.WriteLine("Студент добавлен!");
        }

        public void EditStudent(int id, string? newFullName = null, string? newGroupNameOrId = null)
        {
            var student = _db.Students.Find(id);
            if (student == null) { Console.WriteLine("Студент не найден"); return; }

            if (!string.IsNullOrWhiteSpace(newFullName)) student.FullName = newFullName;

            if (!string.IsNullOrWhiteSpace(newGroupNameOrId))
            {
                Group? group = _db.Groups.FirstOrDefault(g => g.Name == newGroupNameOrId);
                if (group == null && int.TryParse(newGroupNameOrId, out int gid))
                    group = _db.Groups.FirstOrDefault(g => g.Id == gid);

                if (group != null) student.GroupId = group.Id;
            }

            _db.Students.Update(student);
            _db.SaveChanges();
            Console.WriteLine("Студент обновлён!");
        }

        public void DeleteStudent(int id)
        {
            var student = _db.Students.Find(id);
            if (student == null) { Console.WriteLine("Студент не найден"); return; }

            _db.Students.Remove(student);
            _db.SaveChanges();
            Console.WriteLine("Студент удалён!");
        }

        public Student? GetStudentById(int id) =>
            _db.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);
    }
}
