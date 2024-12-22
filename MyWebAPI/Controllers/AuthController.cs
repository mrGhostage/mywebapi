using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    private readonly IConfiguration _config = config;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        if (ValidateUser(login))
        {
            var token = GenerateJwtToken(login.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    private bool ValidateUser(LoginModel login)
    {
        // Здесь должна быть проверка пользователя в базе данных
        return login.Username == "test" && login.Password == "password";
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
