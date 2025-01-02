using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWebAPI.Database.Models.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<StreetViewPoint>
{
    public void Configure(EntityTypeBuilder<StreetViewPoint> builder)
    {
        builder.HasKey(x => new { x.Latitude, x.Longitude });
    }
}