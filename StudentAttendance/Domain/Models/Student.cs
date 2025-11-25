using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Студент — наследует Person.
    /// </summary>
    public class Student : Person
    {
        /// <summary>Id группы (FK).</summary>
        public int GroupId { get; set; }

        /// <summary>Навигационное свойство — группа.</summary>
        public Group? Group { get; set; }

        /// <summary>Список посещаемостей.</summary>
        public List<Attendance> Attendances { get; set; } = new();
    }
}
    