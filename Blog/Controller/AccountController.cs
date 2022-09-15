using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controller
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly BlogDataContext _context;
        public AccountController(TokenService tokenService, BlogDataContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        //Criar usuário
        //senha: PAb5O!$sQon)&ab1pBeap1Ss5
        //user: ruan@flu.com
        //"user": "kratos@god.com"
        //"password": "[ˆiˆSPb*CEl1$nDJ1p72tA*os"
        //"user": "rick@adventure.com",
        //"password": "EHsAT}Lx!ZIZqKHt}20g(8lOX"
        //"user": "miles@spider.com",
        //"password": "tAhdMBCM;OF}M{K61*$br%0p8"
        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> PostAsync([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };


            //Gerador de senha
            var password = PasswordGenerator.Generate(25, true, false);

            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email, password
                }));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(400, new ResultViewModel<Category>("Email já cadastrado."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await _context
                                .Users
                                .AsNoTracking()
                                .Include(x => x.Roles)
                                .FirstOrDefaultAsync( x => x.Email == model.Email);

            if (user == null)
                return StatusCode(400, new ResultViewModel<string>("Usuário ou senha inválidos."));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));

            try
            {
                var token = _tokenService.GenerateToken(user);
                return  Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
            }
        }
    }
}
