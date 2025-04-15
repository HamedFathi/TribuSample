using HamedStack.CQRS;
using HamedStack.TheRepository;
using HamedStack.TheResult;
using Tributech.Domain;

namespace Tributech.Application.Select;

public class GetSensorQueryHandler : IQueryHandler<GetSensorQuery, SensorPoco>
{
    private readonly IRepository<Sensor> _repository;

    public GetSensorQueryHandler(IRepository<Sensor> repository)
    {
        _repository = repository;
    }
    public async Task<Result<SensorPoco>> Handle(GetSensorQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var currentSensor = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (currentSensor == null)
            {
                return Result<SensorPoco>.Failure(null,$"Sensor with {request.Id} id not found.");
            }

            return Result<SensorPoco>.Success(new SensorPoco()
            {
                Id = currentSensor.Id,
                Location = currentSensor.Location,
                LowerWarningLimit = currentSensor.WarningLimit.LowerWarningLimit,
                UpperWarningLimit = currentSensor.WarningLimit.UpperWarningLimit,
                Name = currentSensor.Name
            });
        }
        catch (Exception e)
        {
            return Result<SensorPoco>.Failure(null, e);
        }
    }
}