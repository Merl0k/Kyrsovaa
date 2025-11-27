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
            if (!groups.Any()) { Console.WriteLine("Немає груп"); return; }

            foreach (var g in groups)
                Console.WriteLine($"{g.Id}: {g.Name}");
        }

        public void AddGroup(string name)
        {
            _db.Groups.Add(new Group { Name = name });
            _db.SaveChanges();
            Console.WriteLine($"Група '{name}' успішно додано!");
        }

        public void EditGroupByName(string currentName, string newName)
        {
            var group = _db.Groups.FirstOrDefault(g => g.Name == currentName);
            if (group == null)
            {
                Console.WriteLine($"Група з назвою '{currentName}' не знайдено.");
                return;
            }

            group.Name = newName;
            _db.Groups.Update(group);
            _db.SaveChanges();
            Console.WriteLine($"Група '{currentName}' перейменована в '{newName}'.");
        }


        public void DeleteGroup(int id)
        {
            var group = _db.Groups.Find(id);
            if (group == null) { Console.WriteLine("Група не знайдена"); return; }

            _db.Groups.Remove(group);
            _db.SaveChanges();
            Console.WriteLine("Група видалена!");
        }

        public Group? GetGroupById(int id) => _db.Groups.Find(id);
    }
}
