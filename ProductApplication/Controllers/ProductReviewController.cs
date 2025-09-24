using Microsoft.AspNetCore.Mvc;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductApplication.Controllers
{
    [ApiController]
    [Route("api/product-reviews")]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _service;

        public ProductReviewController(IProductReviewService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new record.")]
        [ProducesResponseType(typeof(ProductReviewCreateDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProductReviewCreateDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.ParamName + " cannot be null" });
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all records.")]
        [ProducesResponseType(typeof(IEnumerable<ProductReviewReadDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieves a specific record by ID.")]
        [ProducesResponseType(typeof(ProductReviewReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                    return NotFound();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing record.")]
        [ProducesResponseType(typeof(ProductReviewUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ProductReviewUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _service.UpdateAsync(dto);
                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.ParamName + " is invalid." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a record by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
    }
}