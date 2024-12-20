using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookmakerBackend.Contracts.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookmakerBackend.AppServices.Services;

///<inheritdoc cref="IJwtService"/>
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Создания экземпляра класса <see cref="JwtService"/>
    /// </summary>
    /// <param name="configuration">Конфигурация.</param>
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    ///<inheritdoc/>
    public string GetToken(LoginUserRequest userData, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userData.Username),
                new Claim(ClaimTypes.Role, role),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}