using HamedStack.CQRS;

namespace Tributech.Application.Delete
{
    public class DeleteSensorCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
