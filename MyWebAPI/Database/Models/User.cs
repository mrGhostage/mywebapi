namespace MyWebAPI.Database.Models;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }
}