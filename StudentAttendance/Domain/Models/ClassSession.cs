using System;
using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Одно занятие (дата, предмет, группа).
    /// </summary>
    public class ClassSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; } = string.Empty;

        /// <summary>Группа, для которой проводилось занятие.</summary>
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public List<Attendance> Attendances { get; set; } = new();
    }
}
