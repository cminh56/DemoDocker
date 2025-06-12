using Microsoft.EntityFrameworkCore;
using DemoDocker_Domain.Entities;
using DemoDocker_Domain.IRepositories;
using DemoDocker_Infrastructure.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace DemoDocker_Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(Product entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            _context.Products.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(Product entity)
        {
            _context.Products.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProductsAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.Products.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.CreatedDate <= endDate.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var sortDirection = isAscending ? "ascending" : "descending";
                query = query.OrderBy($"{sortBy} {sortDirection}");
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedDate);
            }

            // Apply pagination
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}