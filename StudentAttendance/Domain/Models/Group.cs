using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Группа студентов
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Student>? Students { get; set; }
        public ICollection<ClassSession>? Sessions { get; set; }
    }
}
