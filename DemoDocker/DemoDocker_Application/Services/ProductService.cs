using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> AddAsync(CreateProductDTO dto)
        {
            // Validate product
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(AppConstants.Validation.RequiredName);

            if (dto.Price <= 0)
                throw new ArgumentException(AppConstants.Validation.InvalidPrice);

            var product = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDTO>(product);
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

            _mapper.Map(dto, existingProduct);
            _productRepository.Update(existingProduct);
            return _mapper.Map<ProductDTO>(existingProduct);
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