using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Студент
    /// </summary>
    public class Student : Person
    {
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }
    }
}

