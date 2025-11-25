namespace Domain.Models
{
    /// <summary>
    /// Запись посещения: студент, занятие, статус.
    /// </summary>
    public class Attendance
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int ClassSessionId { get; set; }
        public ClassSession? ClassSession { get; set; }

        /// <summary>True — присутствовал, False — отсутствовал.</summary>
        public bool IsPresent { get; set; }
    }
}
