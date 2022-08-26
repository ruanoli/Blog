using Blog.Data;
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
                return Ok(await _context.Categories.ToListAsync());
            } 
            catch (DbUpdateException ex) {
                return StatusCode(500, "Não foi possível encontrar as categorias");
            } catch (Exception ex) {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
            try {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound();

                return Ok(category);
            } 
            catch (DbUpdateException ex) {
                return StatusCode(500, "Não foi possível encontrar a categoria");
            } catch (Exception ex) {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model) {
            try {
                var category = new Category {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };

                await _context.Categories.AddAsync(category);

                if (model == null)
                    return NotFound();

                await _context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", category);
            } 
            catch(DbUpdateException ex) {
                return StatusCode(500,"Não foi possível incluir a categoria");
            }
            catch (Exception ex) {
                return StatusCode(500, "Falha interna no servidor.");
            }
           
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model) {
            try {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound();
                
                category.Name = model.Name;
                category.Slug = model.Slug;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(category);
            } 
            catch (DbUpdateException ex) {
                return StatusCode(500, "Não foi possível atualizar a categoria");
            } catch (Exception ex) {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id) {
            try {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound();

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(category);
            } 
            catch (DbUpdateException ex) {
                return StatusCode(500, "Não foi possível excluir a categoria");
            } catch (Exception ex) {
                return StatusCode(500, "Falha interna no servidor.");
            }
        }
    }
}
