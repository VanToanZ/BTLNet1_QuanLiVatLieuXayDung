using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace BTL_QuanLyVatLieuXayDung.Infrastructure.Common
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly QuanLyVatLieuXayDungDbContext _context;
        private readonly DbSet<T> _dbSet; // Table trong db;
        private static IEnumerable<PropertyInfo> Props => typeof(T).GetProperties();

        public BaseRepository(QuanLyVatLieuXayDungDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        private T TrimData(T entity)
        {
            var stringProperties = entity.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(entity, null)!;
                if (currentValue != null)
                {
                    stringProperty.SetValue(entity, currentValue.Trim(), null);
                }

            }

            return entity;
        }

        public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
            items = includeProperties.Aggregate(items, (current, includeProperty)
                => current.Include(includeProperty));

            return items;
        }


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        {
            return trackChanges == false
                ? _dbSet.AsNoTracking().Where(expression)
                : _dbSet.Where(expression);
        }

        public IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> expression,
            bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindByCondition(expression, trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty)
                => current.Include(includeProperty));
            return items;
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            // var items = _dbSet.AsNoTracking();
            // items = includeProperties.Aggregate(items, (current, includeProperty)
            //     => current.Include(includeProperty));
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                await _context.Entry(entity).Reference(i => includeProperties).LoadAsync();
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }
        public async Task<T?> GetByIdAsync(string id, bool trackChanges = false)
        {
            var data = await _dbSet.FindAsync(id);
            if (data != null && !trackChanges) _context.Entry(data).State = EntityState.Detached;
            return data;
        }


        public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false)
        {
            return !trackChanges ? await _dbSet.AsNoTracking().ToListAsync() : await _dbSet.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id, bool trackChanges = false)
        {
            var data = await _dbSet.FindAsync(id);
            if (data != null && !trackChanges) _context.Entry(data).State = EntityState.Detached;
            return data;
        }
        public T? GetById(int id, bool trackChanges = false)
        {
            var data = _dbSet.Find(id);
            if (data != null && !trackChanges) _context.Entry(data).State = EntityState.Detached;
            return data;
        }
        public async Task<bool> ExitsByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity != null;
        }

        public void Add(T entity)
        {
            entity = TrimData(entity);
            _dbSet.Entry(entity).State = EntityState.Added;
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            var listEntity = entities.ToList();
            for (var i = 0; i < listEntity.Count; i++)
            {
                var entity = listEntity[i];
                listEntity[i] = TrimData(entity);
                _dbSet.Entry(entity).State = EntityState.Added;
            }

            _dbSet.AddRange(listEntity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var listEntity = entities.ToList();
            for (var i = 0; i < listEntity.Count; i++)
            {
                var entity = listEntity[i];
                listEntity[i] = TrimData(entity);
                _dbSet.Entry(entity).State = EntityState.Added;
            }

            await _dbSet.AddRangeAsync(listEntity, cancellationToken);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            entity = TrimData(entity);
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            var listEntity = entities.ToList();
            for (var i = 0; i < listEntity.Count(); i++)
            {
                var entity = listEntity[i];
                listEntity[i] = TrimData(entity);
                _dbSet.Entry(entity).State = EntityState.Modified;
            }

            _dbSet.UpdateRange(listEntity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task SaveDbSetAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        //public async Task BulkSaveDbChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    await _context.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        //}
    }
}