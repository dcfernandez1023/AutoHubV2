using AutoHub.Data;
using AutoHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoHub.Repositories.Abstractions
{
    public abstract class AbstractRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public AbstractRepository(AppDbContext context)
        {
            _context = context;
            // Use reflection to get the DbSet<T> from the context
            var property = context.GetType().GetProperty(typeof(T).Name);
            if (property == null)
            {
                throw new InvalidOperationException($"DbSet property for '{typeof(T).Name}' not found on context '{context.GetType().Name}'.");
            }

            var dbSet = property.GetValue(context) as DbSet<T>;
            if (dbSet == null)
            {
                throw new InvalidOperationException($"DbSet property for '{typeof(T).Name}' is not of type DbSet<{typeof(T).Name}>.");
            }

            _dbSet = dbSet;
        }

        public async Task<T> AddAsync(T record)
        {
            _dbSet.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Guid?> DeleteAsync(Guid id)
        {
            T? record = await _dbSet.FindAsync(id);
            if (record != null)
            {
                _dbSet.Remove(record);
                await _context.SaveChangesAsync();
                return id;
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetByUserId(Guid userId)
        {
            var parameter = Expression.Parameter(typeof(T), "v");
            var property = Expression.Property(parameter, "UserId");
            var constant = Expression.Constant(userId, typeof(Guid));
            var equality = Expression.Equal(property, constant);

            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await _dbSet.Where(lambda).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T record)
        {
            _dbSet.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }
    }
}
