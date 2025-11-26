namespace Domain.Models
{
    /// <summary>
    /// јбстрактна€ сущность Person Ч базовый класс дл€ Student и т.д.
    /// </summary>
    public abstract class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
