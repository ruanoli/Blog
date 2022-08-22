using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controller {
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly BlogDataContext _context;
        public CategoryController(BlogDataContext context) {
            _context = context;
        }

        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync() => Ok(await _context.Categories.ToListAsync());

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] Category model) {
           await _context.Categories.AddAsync(model);

            if (model == null)
                return NotFound();

            await _context.SaveChangesAsync();

            return Created($"v1/categories/{model.Id}", model);
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model) {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            category.Name = model.Name;
            category.Slug = model.Slug; 

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id) { 
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}
