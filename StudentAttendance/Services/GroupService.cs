using Infrastructure;
using Domain.Models;
using System;
using System.Linq;

namespace Services
{
    public class GroupService
    {
        private readonly AppDbContext _db;

        public GroupService(AppDbContext db)
        {
            _db = db;
        }

        public void ListGroups()
        {
            var groups = _db.Groups.ToList();
            if (!groups.Any())
            {
                Console.WriteLine("Нет групп.");
                return;
            }

            foreach (var g in groups)
                Console.WriteLine($"{g.Id}: {g.Name} (студентов: {g.Students?.Count ?? 0})");
        }

        public void AddGroup(string name)
        {
            if (_db.Groups.Any(g => g.Name == name))
            {
                Console.WriteLine("Такая группа уже существует!");
                return;
            }

            var group = new Group { Name = name };
            _db.Groups.Add(group);
            _db.SaveChanges();

            Console.WriteLine($"Группа '{name}' успешно добавлена!");
        }
    }
}
