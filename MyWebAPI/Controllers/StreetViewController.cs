using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Services.GoogleMapsSigner;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StreetViewController(ILogger<StreetViewController> logger, HttpClient httpClient) : ControllerBase
{
    //private readonly HttpClient _httpClient;
    private static readonly string apiKey = "AIzaSyBZnSdV279IzjcYr67jnx26Jk1-A3ygqH0";
    private static readonly string secretKey = "LEWw_588MufbbdOfXE1QPpNR-dI=";
    private static readonly string url = "https://maps.googleapis.com/maps/api/streetview";

    private readonly ILogger<StreetViewController> _logger = logger;

    public class StreetViewMetadata
    {
        public string status { get; set; }
        public string pano_id { get; set; }
        public LocationPoint location { get; set; }

        public class LocationPoint
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }

    [Authorize]
    [HttpGet("GetPanoId")]
    public async Task<IActionResult> GetPanoId()
    {
        return Content("cKDwWgvb9g66qP9jf_zzHw");
    }


    [HttpPost("Generate")]
    public async Task<IActionResult> Generate()
    {
        double minLatitude = -47.0;
        double maxLatitude = 66.0;

        double testLat = 51.532840;
        double testLong = 46.021178;

        string url = $"https://maps.googleapis.com/maps/api/streetview/metadata?location={testLat.ToString("F6", CultureInfo.InvariantCulture)},{testLong.ToString("F6", CultureInfo.InvariantCulture)}&key={apiKey}";

        string signedUrl = GoogleMapsSigner.Sign(url, secretKey);


        using HttpClient client = new();
        HttpResponseMessage response = await client.GetAsync(signedUrl);
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error fetching data from Google Maps API");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();
        var metadata = JsonSerializer.Deserialize<StreetViewMetadata>(jsonResponse);

        if (metadata.status == "OK")
        {
            return Ok(new
            {
                PanoramaId = metadata.pano_id,
                Location = metadata.location,
                Status = metadata.status
            });
        }
        else
        {
            return NotFound(jsonResponse);
        }

        // HttpResponseMessage response = await _httpClient.GetAsync(url);
        // if (!response.IsSuccessStatusCode)
        // {
        //     return StatusCode((int)response.StatusCode, "Error fetching data from Google Maps API");
        // }

        // string jsonResponse = await response.Content.ReadAsStringAsync();
        // var metadata = JsonSerializer.Deserialize<StreetViewMetadata>(jsonResponse);

        // if (metadata.Status == "OK")
        // {
        //     return Ok(new
        //     {
        //         PanoramaId = metadata.PanoId,
        //         Location = metadata.Location,
        //         Status = metadata.Status
        //     });
        // }
        // else
        // {
        //     return NotFound("No nearby panorama found");
        // }


        // string parameters = $"?size=600x300&location={testLat.ToString("F6", CultureInfo.InvariantCulture)},{testLong.ToString("F6", CultureInfo.InvariantCulture)}&fov=90&heading=235&pitch=10&key={apiKey}";

        // using (HttpClient client = new HttpClient())
        // {
        //     HttpResponseMessage response = await client.GetAsync(url + parameters);
        //     if (response.IsSuccessStatusCode)
        //     {
        //         byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
        //         System.IO.File.WriteAllBytes("streetview.jpg", imageBytes);
        //         Console.WriteLine("Street View image saved as streetview.jpg");
        //     }
        //     else
        //     {
        //         Console.WriteLine($"Error: {response.StatusCode}");
        //     }
        // }

        // return Ok();
    }
}
