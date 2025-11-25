using System;

namespace Domain.Models
{
    /// <summary>
    /// Базовая абстрактная сущность для людей.
    /// </summary>
    public abstract class Person
    {
        /// <summary>Идентификатор.</summary>
        public int Id { get; set; }

        /// <summary>ФИО.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Дата регистрации/рождения (опционально).</summary>
        public DateTime? DateInfo { get; set; }
    }
}
