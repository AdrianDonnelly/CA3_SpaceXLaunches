using System.Net.Http.Json;
using System.Text.Json.Serialization;


namespace X00194620_Ca3.Services
{
    public class SpaceXService
    {
        private readonly HttpClient _http;

        public SpaceXService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Launch>> GetLaunchesAsync()
        {
            return await _http.GetFromJsonAsync<List<Launch>>(
                "https://api.spacexdata.com/v5/launches"
            ) ?? new List<Launch>();
        }


        public async Task<Launch?> GetLaunchAsync(string id)
        {
            return await _http.GetFromJsonAsync<Launch>(
                $"https://api.spacexdata.com/v5/launches/{id}"
            );
        }
    }

    public class Launch
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date_utc")]
        public DateTime? DateUtc { get; set; }

        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }
    }
    public class Links
    {
        [JsonPropertyName("patch")]
        public Patch Patch { get; set; }

        [JsonPropertyName("flickr")]
        public Flickr Flickr { get; set; }

        [JsonPropertyName("presskit")]
        public string Presskit { get; set; }

        [JsonPropertyName("webcast")]
        public string Webcast { get; set; }

        [JsonPropertyName("article")]
        public string Article { get; set; }

        [JsonPropertyName("wikipedia")]
        public string Wikipedia { get; set; }
    }
    
    public class Patch
    {
        [JsonPropertyName("small")]
        public string Small { get; set; }

        [JsonPropertyName("large")]
        public string Large { get; set; }
    }
    
    public class Flickr
    {
        [JsonPropertyName("small")]
        public List<string> Small { get; set; }

        [JsonPropertyName("original")]
        public List<string> Original { get; set; }
    }
}