using Microsoft.AspNetCore.Mvc;
using DemoDocker_Application.Services;
using DemoDocker_Common.DTO;
using DemoDocker_Common.Constants;
using DemoDocker_Common.Common;
using AutoMapper;
using DemoDocker_Domain.Entities;

namespace DemoDocker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);
                return Ok(new ApiResponse<IEnumerable<ProductDTO>>(200, ResponseKeys.Success, productDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, ResponseKeys.ErrorSystem, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new ApiResponse<string>(404, ResponseKeys.NotFound, 
                        string.Format(AppConstants.Validation.ProductNotFound, id)));

                var productDto = _mapper.Map<ProductDTO>(product);
                return Ok(new ApiResponse<ProductDTO>(200, ResponseKeys.Success, productDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, ResponseKeys.ErrorSystem, ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
        {
            try
            {
                var product = _mapper.Map<Product>(dto);
                var createdProduct = await _productService.AddAsync(product);
                var productDto = _mapper.Map<ProductDTO>(createdProduct);
                
                return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, 
                    new ApiResponse<ProductDTO>(201, ResponseKeys.Created, productDto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(400, ResponseKeys.ValidationError, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, ResponseKeys.ErrorSystem, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDTO dto)
        {
            try
            {
                var product = _mapper.Map<Product>(dto);
                var updatedProduct = await _productService.UpdateAsync(id, product);
                var productDto = _mapper.Map<ProductDTO>(updatedProduct);
                
                return Ok(new ApiResponse<ProductDTO>(200, ResponseKeys.Updated, productDto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(404, ResponseKeys.NotFound, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(400, ResponseKeys.ValidationError, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, ResponseKeys.ErrorSystem, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.RemoveAsync(id);
                return Ok(new ApiResponse<string>(200, ResponseKeys.Deleted, null));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(404, ResponseKeys.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, ResponseKeys.ErrorSystem, ex.Message));
            }
        }
    }
} 