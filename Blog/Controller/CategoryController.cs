using Blog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller {
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly BlogDataContext _context;
        public CategoryController(BlogDataContext context) {
            _context = context;
        }

        [HttpGet("categories")]
        public IActionResult Get() => Ok(_context.Categories.ToList());

    }
}
