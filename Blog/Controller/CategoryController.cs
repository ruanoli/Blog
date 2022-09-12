using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
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
        public async Task<IActionResult> GetAsync() {
            try {
                var categories = await _context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            } 
            catch{
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
            try {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            } catch {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context) {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try {
                var category = new Category {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower(),
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            } catch (DbUpdateException ex) {
                return StatusCode(400, new ResultViewModel<Category>("Não foi possível criar a categoria"));
            } catch {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model) {
            try {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));
                
                category.Name = model.Name;
                category.Slug = model.Slug;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            } 
            catch (DbUpdateException ex) {
                return StatusCode(400, new ResultViewModel<Category>("Não foi possível atualizar a categoria."));
            } catch (Exception ex) {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id) {
            try {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            } 
            catch (DbUpdateException ex) {
                return StatusCode(400, new ResultViewModel<Category>("Não foi possível atualizar a categoria."));
            } catch (Exception ex) {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }
    }
}
