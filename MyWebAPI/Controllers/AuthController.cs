using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyWebAPI.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration config, ApplicationContext context) : ControllerBase
{
    private readonly IConfiguration _config = config;
    private readonly ApplicationContext _context = context;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        if (await ValidateUser(login))
        {
            var token = GenerateJwtToken(login.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var test = User.Claims.ToList();
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); // Обычно идентификатор пользователя хранится в claim "sub"

        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        var login = userIdClaim.Value;

        //Ищем пользователя в базе данных
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(user);
    }

    private async Task<bool> ValidateUser(LoginModel login)
    {
        // Здесь должна быть проверка пользователя в базе данных
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == login.Username);
        return user != null && user.Password == login.Password;
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
