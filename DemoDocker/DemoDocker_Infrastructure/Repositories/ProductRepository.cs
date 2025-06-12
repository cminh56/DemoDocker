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
    }
}