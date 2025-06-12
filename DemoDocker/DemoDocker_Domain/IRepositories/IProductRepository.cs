using DemoDocker_Domain.Entities;

namespace DemoDocker_Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product entity);
        void Update(Product entity);
        void Remove(Product entity);
    }
} 