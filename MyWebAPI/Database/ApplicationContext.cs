using Microsoft.EntityFrameworkCore;
using MyWebAPI.Database.Models;

namespace MyWebAPI.Database;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<StreetViewPoint> StreetViewPoints { get; set; }


    // public ApplicationContext()
    // {
    //     Database.EnsureCreated();
    // }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     //optionsBuilder.UseNpgsql("Host=dpg-ctjqg3ij1k6c73cl5fc0-a;Port=5432;Database=test_web_api_database;Username=test_web_api_database_user;Password=yGMKe0Plx63OvFKaz37botNfgpw6uv1T");
    // }
}