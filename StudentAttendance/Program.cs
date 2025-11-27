using Infrastructure;
using Services;
using UI;
using Data;

class Program
{
    static void Main()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();
        SeedData.Initialize(db);

        var studentService = new StudentService(db);
        var groupService = new GroupService(db);
        var attendanceService = new AttendanceService(db);

        var ui = new ConsoleUI(studentService, attendanceService, groupService);
        ui.Run();
    }
}
