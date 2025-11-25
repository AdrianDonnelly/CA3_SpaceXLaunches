namespace X00194620_Ca3.Services;

using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<object> GetRootAsync()
    {
        return await _http.GetFromJsonAsync<object>("https://api.arcsecond.io/");
    }
    
    public async Task<List<ArcObject>> SearchObjects(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<ArcObject>();

        string url = $"https://api.arcsecond.io/objects?q={query}&limit=10";

        var result = await _http.GetFromJsonAsync<ArcObjectResponse>(url);
        return result?.Results ?? new List<ArcObject>();
    }

    public class ArcObjectResponse
    {
        public List<ArcObject> Results { get; set; }
        public int Count { get; set; }
    }

    public class ArcObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
    }
}
