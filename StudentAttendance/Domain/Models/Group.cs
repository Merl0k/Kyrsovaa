using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Группа студентов.
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>Студенты группы.</summary>
        public List<Student> Students { get; set; } = new();
    }
}
