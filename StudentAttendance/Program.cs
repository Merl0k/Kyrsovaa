using Infrastructure;
using Services;
using UI;
using Data;

class Program
{
    static void Main()
    {
        using var db = new AppDbContext();

        // Для разработки: применить миграции (если уже добавлены)
        // Если будешь применять миграции через dotnet ef, строка ниже не нужна.
        db.Database.EnsureCreated();

        // Залить seed данные, если БД пустая
        SeedData.Initialize(db);

        var service = new AttendanceService(db);
        var ui = new ConsoleUI(service);
        ui.Run();
    }
}
