using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        //Criar usuário
        [HttpPost("v1/accounts/")]
        //public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        //    var user = new User
        //    {
        //        Name = model.Name,
        //        Email = model.Email,
        //        Slug = model.Email.Replace("@", "-").Replace(".", "-")
        //    };


        //}

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login()
        {
            var token = _tokenService.GenerateToken(null);

            return Ok(token);
        }
    }
}
