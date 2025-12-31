using Aban.DataLayer.Context;
using Aban.DataLayer.Interfaces.Generics;
using Microsoft.EntityFrameworkCore;

namespace Aban.DataLayer.Repositories.Generics
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region property
        private readonly AppDbContext _applicationDbContext;
        private DbSet<T> entities;
        #endregion
        #region Constructor
        public GenericRepository(AppDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            entities = _applicationDbContext.Set<T>();
        }
        #endregion
        public IQueryable<T> GetAll() => entities;

        /// <summary>
        /// with SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delete(T entity)
        {
            _applicationDbContext.Remove(entity);
            _applicationDbContext.SaveChanges();
        }

        /// <summary>
        /// with SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeleteRange(IEnumerable<T> models)
        {
            entities.UpdateRange(models);
            _applicationDbContext.SaveChanges();
        }

#pragma warning disable CS8603 // Possible null reference return.
        //public T Find(Guid Id) => entities.SingleOrDefault(c => c.Equals(Id));
        public T Find(Guid id) => _applicationDbContext.Set<T>().Find(id);
        public T Find(string id) => _applicationDbContext.Set<T>().Find(id);
        public async Task<T> FindAsync(string id) => await _applicationDbContext.Set<T>().FindAsync(id);
        public T Find(int id) => _applicationDbContext.Set<T>().Find(id);
        public async Task<T> FindAsync(int id) => await _applicationDbContext.Set<T>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _applicationDbContext.SaveChanges();
        }

        public void InsertRange(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.AddRange(entity);
            _applicationDbContext.SaveChanges();
        }

        /// <summary>
        /// without SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }

        /// <summary>
        /// without SaveChanges()
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveRange(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.RemoveRange(entity);
        }

        public async Task SaveChangesAsync() => await _applicationDbContext.SaveChangesAsync();
        public void SaveChanges() => _applicationDbContext.SaveChanges();


        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Attach(entity);
            entities.Update(entity);


            //_applicationDbContext.SaveChanges();
        }

        public void Update(T entity, bool usSaveChange)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Attach(entity);
            entities.Update(entity);


            if (usSaveChange)
                _applicationDbContext.SaveChanges();
            //_applicationDbContext.SaveChanges();
        }

        public async Task UpdateAsync(T model, bool usSaveChange = true)
        {
            if (model is null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Attach(model);
            entities.Update(model);
            if (usSaveChange)
                await _applicationDbContext.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<T> entity, bool isSaveChange = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.UpdateRange(entity);

            if (isSaveChange)
                _applicationDbContext.SaveChanges();
        }

        public virtual async Task<T> GetByIdStringAsync(string id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _applicationDbContext.Set<T>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
