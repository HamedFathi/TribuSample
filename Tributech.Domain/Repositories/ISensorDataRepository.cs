namespace Tributech.Domain.Repositories;

public interface ISensorDataRepository
{
    Task<List<SensorData>?> GetSensorDataAsync(string streamId, DateTimeOffset fromTimestamp, DateTimeOffset toTimestamp);
}