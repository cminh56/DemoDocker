using DemoDocker_Domain.Entities;

namespace DemoDocker_Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product entity);
        void Update(Product entity);
        void Remove(Product entity);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10);
    }
} 