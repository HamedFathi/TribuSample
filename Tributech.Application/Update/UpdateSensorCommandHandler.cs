using FluentValidation;
using HamedStack.CQRS;
using HamedStack.TheRepository;
using HamedStack.TheResult;
using HamedStack.TheResult.FluentValidation;
using Tributech.Domain;

namespace Tributech.Application.Update;

public class UpdateSensorCommandHandler : ICommandHandler<UpdateSensorCommand, bool>
{
    private readonly IRepository<Sensor> _repository;
    private readonly IValidator<UpdateSensorCommand> _validator;

    public UpdateSensorCommandHandler(IRepository<Sensor> repository, IValidator<UpdateSensorCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<Result<bool>> Handle(UpdateSensorCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<bool>();
        }

        var currentSensor = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (currentSensor == null)
        {
            return Result<bool>.Failure($"Sensor with {request.Id} id not found.");
        }

        var newSensor = Sensor.Create(request.Name, request.Location, DateTimeOffset.Now, request.LowerWarningLimit,
            request.UpperWarningLimit);
        if (!newSensor.IsSuccess)
        {
            return Result<bool>.Failure(newSensor.Errors);
        }

        currentSensor.WarningLimit = newSensor.Value!.WarningLimit;
        currentSensor.Location = newSensor.Value!.Location;
        currentSensor.Name = newSensor.Value!.Name;

        try
        {
            await _repository.UpdateAsync(currentSensor!, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e);
        }
    }
}