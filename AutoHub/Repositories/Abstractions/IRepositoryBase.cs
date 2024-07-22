using AutoHub.Models;

namespace AutoHub.Repositories.Abstractions
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetByUserId(Guid userId);
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T record);
        Task<T> UpdateAsync(T record);
        Task<Guid?> DeleteAsync(Guid id);
    }
}
