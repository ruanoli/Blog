using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller {
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase {

        [HttpGet("")]
        public async Task<IActionResult> GetAsync() {
            return Ok();
        }
    }
}
