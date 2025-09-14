using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.Entities;
using ProductAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product.")]
        [ProducesResponseType(typeof(ProductCreateDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO dto)
        {
            if (!ModelState.IsValid)
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
        [SwaggerOperation(Summary = "Retrieves all products.")]
        [ProducesResponseType(typeof(IEnumerable<ProductReadDTO>), StatusCodes.Status200OK)]
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
        [SwaggerOperation(Summary = "Retrieves a specific product by ID.")]
        [ProducesResponseType(typeof(ProductReadDTO), StatusCodes.Status200OK)]
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
        [SwaggerOperation(Summary = "Updates an existing product.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDTO dto)
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
        [SwaggerOperation(Summary = "Deletes a product by ID.")]
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