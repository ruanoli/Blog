using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controller
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public AccountController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("v1/login")]
        public async Task<IActionResult> Login()
        {
            var token = _tokenService.GenerateToken(null);

            return Ok(token);
        }
    }
}
