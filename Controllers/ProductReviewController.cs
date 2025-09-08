using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.Entities;
using ProductAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product review.")]
        [ProducesResponseType(typeof(ProductReviewCreateDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProductReviewCreateDTO object)
        {
            var result = await _productReviewService.CreateAsync(object);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all product reviews.")]
        [ProducesResponseType(typeof(IEnumerable<ProductReviewReadDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productReviewService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieves a specific product review by ID.")]
        [ProducesResponseType(typeof(ProductReviewReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productReviewService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing product review.")]
        [ProducesResponseType(typeof(ProductReviewUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ProductReviewUpdateDTO object)
        {
            var result = await _productReviewService.UpdateAsync(object);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a product review by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productReviewService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}