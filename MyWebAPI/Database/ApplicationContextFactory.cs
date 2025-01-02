using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyWebAPI.Database;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseNpgsql("Host=dpg-ctjqg3ij1k6c73cl5fc0-a.oregon-postgres.render.com;Port=5432;Database=test_web_api_database;Username=test_web_api_database_user;Password=yGMKe0Plx63OvFKaz37botNfgpw6uv1T;SslMode=Require;Trust Server Certificate=true");

        return new ApplicationContext(optionsBuilder.Options);
    }
}