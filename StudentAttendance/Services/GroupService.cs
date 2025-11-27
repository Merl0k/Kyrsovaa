using Infrastructure;
using Domain.Models;
using System;
using System.Linq;

namespace Services
{
    public class GroupService
    {
        private readonly AppDbContext _db;
        public GroupService(AppDbContext db) => _db = db;

        public void ListGroups()
        {
            var groups = _db.Groups.ToList();
            if (!groups.Any()) { Console.WriteLine("Нет групп"); return; }

            foreach (var g in groups)
                Console.WriteLine($"{g.Id}: {g.Name}");
        }

        public void AddGroup(string name)
        {
            _db.Groups.Add(new Group { Name = name });
            _db.SaveChanges();
            Console.WriteLine($"Группа '{name}' успешно добавлена!");
        }

        public void EditGroupByName(string currentName, string newName)
        {
            var group = _db.Groups.FirstOrDefault(g => g.Name == currentName);
            if (group == null)
            {
                Console.WriteLine($"Группа с именем '{currentName}' не найдена.");
                return;
            }

            group.Name = newName;
            _db.Groups.Update(group);
            _db.SaveChanges();
            Console.WriteLine($"Группа '{currentName}' переименована в '{newName}'.");
        }


        public void DeleteGroup(int id)
        {
            var group = _db.Groups.Find(id);
            if (group == null) { Console.WriteLine("Группа не найдена"); return; }

            _db.Groups.Remove(group);
            _db.SaveChanges();
            Console.WriteLine("Группа удалена!");
        }

        public Group? GetGroupById(int id) => _db.Groups.Find(id);
    }
}
