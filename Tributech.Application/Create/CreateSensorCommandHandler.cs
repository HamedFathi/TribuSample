using FluentValidation;
using HamedStack.CQRS;
using HamedStack.TheRepository;
using HamedStack.TheResult;
using HamedStack.TheResult.FluentValidation;
using Tributech.Domain;

namespace Tributech.Application.Create;

public class CreateSensorCommandHandler : ICommandHandler<CreateSensorCommand, Guid>
{
    private readonly IRepository<Sensor> _repository;
    private readonly IValidator<CreateSensorCommand> _validator;

    public CreateSensorCommandHandler(IRepository<Sensor> repository, IValidator<CreateSensorCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<Result<Guid>> Handle(CreateSensorCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<Guid>();
        }

        var sensor = Sensor.Create(request.Name, request.Location, DateTimeOffset.Now, request.LowerWarningLimit,
            request.UpperWarningLimit);
        if (!sensor.IsSuccess)
        {
            return Result<Guid>.Failure(Guid.Empty, sensor.Errors);
        }

        try
        {
            await _repository.AddAsync(sensor!, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(sensor.Value!.Id);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure(Guid.Empty, e);
        }
    }
}