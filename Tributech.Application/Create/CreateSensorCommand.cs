using HamedStack.CQRS;

namespace Tributech.Application.Create
{
    public class CreateSensorCommand : ICommand<Guid>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public double UpperWarningLimit { get; set; }
        public double LowerWarningLimit { get; set; }
    }
}
