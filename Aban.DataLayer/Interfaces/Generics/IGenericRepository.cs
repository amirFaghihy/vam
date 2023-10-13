namespace Aban.DataLayer.Interfaces.Generics
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        void Delete(T model);
        void DeleteRange(IEnumerable<T> models);
        T Find(Guid Id);
        T Find(string Id);
        Task<T> FindAsync(string Id);
        T Find(int Id);
        Task<T> FindAsync(int Id);
        void Insert(T model);
        void InsertRange(IEnumerable<T> model);
        void Remove(T model);
        void RemoveRange(IEnumerable<T> model);
        Task SaveChangesAsync();
        void SaveChanges();
        void Update(T model);
        void Update(T model, bool usSaveChange);
        Task UpdateAsync(T model, bool usSaveChange = true);
        void UpdateRange(IEnumerable<T> model, bool isSaveChange = true);
    }
}
