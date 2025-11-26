namespace Domain.Models
{
    /// <summary>
    /// Посещение: связь студент <-> занятие
    /// </summary>
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int ClassSessionId { get; set; }
        public ClassSession? ClassSession { get; set; }

        public bool IsPresent { get; set; } = false;
    }
}
