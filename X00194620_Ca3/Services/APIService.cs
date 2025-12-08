using System.Net.Http.Json;

namespace X00194620_Ca3.Services
{
    public class SpaceXService
    {
        private readonly HttpClient _http;

        public SpaceXService(HttpClient http)
        {
            _http = http;
        }

        // Fetch all launches
        public async Task<List<Launch>> GetLaunchesAsync()
        {
            var launches = await _http.GetFromJsonAsync<List<Launch>>(
                "https://api.spacexdata.com/v4/launches"
            );

            return launches ?? new List<Launch>();
        }

        // Fetch a single launch by ID
        public async Task<Launch?> GetLaunchAsync(string id)
        {
            return await _http.GetFromJsonAsync<Launch>(
                $"https://api.spacexdata.com/v4/launches/{id}"
            );
        }
    }

    // ===================== MODELS =====================

    public class Launch
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime DateUtc { get; set; }
        public bool? Success { get; set; }
        public string? Details { get; set; }
        public string Rocket { get; set; } = "";
        public Links Links { get; set; } = new();
    }

    public class Links
    {
        public Patch Patch { get; set; } = new();
        public Flickr Flickr { get; set; } = new();
    }

    public class Patch
    {
        public string? Small { get; set; }
        public string? Large { get; set; }
    }

    public class Flickr
    {
        public List<string> Original { get; set; } = new();
    }
}