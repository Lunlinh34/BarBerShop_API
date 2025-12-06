using System.Linq.Expressions;

namespace BarberShop.Repository
{
    public interface IRepository<T> where T : class
    {
        //T GetById(int id);
        TDto GetById<TDto>(int id);
        public Task Add(T entity);
        Task UpdateProperties(int id, Action<T> updateAction);
        public Task Delete(int id);
        IEnumerable<TDto> GetAll<TDto>();
        IEnumerable<TDto> Search<TDto>(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T GetEntityById(int id); // 👈 thêm dòng này
        IQueryable<T> GetQuery(); // 👈 thêm dòng này
        Task AddAsync(T entity); // 🟢 thêm dòng này
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task DeleteAsync(int id); // 👈 thêm dòng này

    }
}
