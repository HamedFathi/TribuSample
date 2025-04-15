using HamedStack.CQRS;
using HamedStack.TheRepository;
using HamedStack.TheResult;
using Tributech.Domain;

namespace Tributech.Application.SelectAll;

public class GetSensorsQueryHandler : IQueryHandler<GetSensorsQuery, IEnumerable<SensorPoco>>
{
    private readonly IRepository<Sensor> _repository;

    public GetSensorsQueryHandler(IRepository<Sensor> repository)
    {
        _repository = repository;
    }
    public async Task<Result<IEnumerable<SensorPoco>>> Handle(GetSensorsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var currentSensor = await _repository.GetAll(cancellationToken);

            var sensors = new List<SensorPoco>();
            sensors.AddRange(currentSensor.Select(ConvertTo));

            return Result<IEnumerable<SensorPoco>>.Success(sensors);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<SensorPoco>>.Failure(null,e);
        }
    }

    private static SensorPoco ConvertTo(Sensor sensor)
    {
        return new SensorPoco()
        {
            Name = sensor.Name,
            Location = sensor.Location,
            Id = sensor.Id,
            LowerWarningLimit = sensor.WarningLimit.LowerWarningLimit,
            UpperWarningLimit = sensor.WarningLimit.UpperWarningLimit
        };
    }
}