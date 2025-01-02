using Microsoft.EntityFrameworkCore;

namespace MyWebAPI.Database.Models;

[PrimaryKey(nameof(Latitude), nameof(Longitude))]
public class StreetViewPoint
{
    /// <summary>
    /// Широта
    /// </summary>
    public double Latitude { get; set; }
    /// <summary>
    /// Долгота
    /// </summary>
    public double Longitude { get; set; }
    /// <summary>
    /// Количество использований
    /// </summary>
    public int UsedCount { get; set; }
}