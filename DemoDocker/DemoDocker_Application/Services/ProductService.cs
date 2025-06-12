using DemoDocker_Common;
using DemoDocker_Common.Constants;
using DemoDocker_Common.DTO;
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

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDTO);
        }

        public async Task<ProductDTO?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDTO(product);
        }

        public async Task<ProductDTO> AddAsync(CreateProductDTO dto)
        {
            // Validate product
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(AppConstants.Validation.RequiredName);

            if (dto.Price <= 0)
                throw new ArgumentException(AppConstants.Validation.InvalidPrice);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            await _productRepository.AddAsync(product);
            return MapToDTO(product);
        }

        public async Task<ProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            // Validate product
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(AppConstants.Validation.RequiredName);

            if (dto.Price <= 0)
                throw new ArgumentException(AppConstants.Validation.InvalidPrice);

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException(string.Format(AppConstants.Validation.ProductNotFound, id));

            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;

            _productRepository.Update(existingProduct);
            return MapToDTO(existingProduct);
        }

        public async Task RemoveAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException(string.Format(AppConstants.Validation.ProductNotFound, id));

            _productRepository.Remove(product);
        }

        private static ProductDTO MapToDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedDate = product.CreatedDate,
                UpdateDate = product.UpdateDate
            };
        }
    }
} 