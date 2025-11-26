using System;
using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// Занятие / пара
    /// </summary>
    public class ClassSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Topic { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }
    }
}
