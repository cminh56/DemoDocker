using DemoDocker_Common.Constants;
using DemoDocker_Domain.Entities;
using DemoDocker_Domain.IRepositories;

namespace DemoDocker_Application.Services
{
    public class ProductService 
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            // Validate product
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException(AppConstants.Validation.RequiredName);

            if (product.Price <= 0)
                throw new ArgumentException(AppConstants.Validation.InvalidPrice);

            await _productRepository.AddAsync(product);
            return product;
        }

        public async Task<Product> UpdateAsync(Guid id, Product product)
        {
            // Validate product
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException(AppConstants.Validation.RequiredName);

            if (product.Price <= 0)
                throw new ArgumentException(AppConstants.Validation.InvalidPrice);

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException(string.Format(AppConstants.Validation.ProductNotFound, id));

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            
            _productRepository.Update(existingProduct);
            return existingProduct;
        }

        public async Task RemoveAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException(string.Format(AppConstants.Validation.ProductNotFound, id));

            _productRepository.Remove(product);
        }
    }
} 