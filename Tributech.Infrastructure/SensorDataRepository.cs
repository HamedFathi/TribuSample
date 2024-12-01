using System.Text.Json;
using Tributech.Domain;
using Tributech.Domain.Repositories;

namespace Tributech.Infrastructure
{
    public class SensorDataRepository : ISensorDataRepository
    {
        private readonly HttpClient _httpClient;

        public SensorDataRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SensorData>?> GetSensorDataAsync(string streamId, DateTimeOffset fromTimestamp, DateTimeOffset toTimestamp)
        {
            var url = $"https://testplatform.io/values/double?StreamId={streamId}&From={fromTimestamp:yyyy-MM-ddTHH:mm:ss.fffZ}&To={toTimestamp:yyyy-MM-ddTHH:mm:ss.fffZ}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<SensorData>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
