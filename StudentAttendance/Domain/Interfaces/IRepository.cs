using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// ќбщий репозиторий Ч интерфейс (generic).
    /// </summary>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
