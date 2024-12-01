namespace Tributech.Domain.Services;

public interface ISensorDataService
{
    Task<List<SensorData>?> GetSensorDataAsync(string streamId, DateTimeOffset fromTimestamp, DateTimeOffset toTimestamp);
}