using HamedStack.CQRS;
using HamedStack.TheRepository;
using HamedStack.TheResult;
using Tributech.Domain;

namespace Tributech.Application.Delete;

public class DeleteSensorCommandHandler : ICommandHandler<DeleteSensorCommand, bool>
{
    private readonly IRepository<Sensor> _repository;

    public DeleteSensorCommandHandler(IRepository<Sensor> repository)
    {
        _repository = repository;
    }
    public async Task<Result<bool>> Handle(DeleteSensorCommand request, CancellationToken cancellationToken)
    {
        var currentSensor = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (currentSensor == null)
        {
            return Result<bool>.Failure($"Sensor with {request.Id} id not found.");
        }

        try
        {
            await _repository.DeleteAsync(currentSensor, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e);
        }
    }
}