using Tributech.Domain;
using Tributech.Domain.Repositories;
using Tributech.Domain.Services;

namespace Tributech.Application.Service
{
    public class SensorDataService : ISensorDataService
    {
        private readonly ISensorDataRepository _repository;

        public SensorDataService(ISensorDataRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SensorData>?> GetSensorDataAsync(string streamId, DateTimeOffset fromTimestamp, DateTimeOffset toTimestamp)
        {
            return _repository.GetSensorDataAsync(streamId, fromTimestamp, toTimestamp);
        }
    }
}
