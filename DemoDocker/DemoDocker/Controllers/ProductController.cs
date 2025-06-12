using Microsoft.AspNetCore.Mvc;
using DemoDocker_Application.Services;
using DemoDocker_Common.DTO;
using DemoDocker_Common.Constants;
using DemoDocker_Common.Common;

namespace DemoDocker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<ProductDTO>>(200, ResponseKeys.Success, products));
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

                return Ok(new ApiResponse<ProductDTO>(200, ResponseKeys.Success, product));
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
                var product = await _productService.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, 
                    new ApiResponse<ProductDTO>(201, ResponseKeys.Created, product));
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
                var product = await _productService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<ProductDTO>(200, ResponseKeys.Updated, product));
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