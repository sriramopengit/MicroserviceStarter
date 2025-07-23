// Controllers/InventoryController.cs
using InventoryService.Interfaces;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var j = 10 *  0;
            var k = 100 / j;
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InventoryItem item)
        {
            var validator = new InventoryItemValidator();
            var validationResult = await validator.ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                                .Select(e => e.ErrorMessage)
                                .ToList();

                return BadRequest(errors);
            }

            var addedItem = await _service.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = addedItem.Id }, addedItem);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]InventoryItem item)
        {
            item.Id = id;

            if (id != item.Id) return BadRequest("Mismatched ID");

            var updated = await _service.UpdateAsync(item);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
