using Blog.Extensions;
using Blog.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Services
{
    public class TokenService
    {
        //Gerando token pro usuário token
        public string GenerateToken(User user)
        {
            //manipulador de token
            var tokenHandler = new JwtSecurityTokenHandler();

            //chave
            //O tokenHandler espera um array de bytes e não uma string.
            //convertendo a chave para bytes:
            var key = Encoding.ASCII.GetBytes(Settings.JwtToken);

            var claims = user.GetClaims();

            //configurações do token, contém todas as informações do token.
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),

                //Definir o tempo de duração do token
                Expires = DateTime.UtcNow.AddHours(8),

                //como token é gerado e lido posteriormente. Encriptando e desencriptando.
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)

            };

            //gerando o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            //WriteToken retorna uma string
            return tokenHandler.WriteToken(token); 
        }
    }
}
