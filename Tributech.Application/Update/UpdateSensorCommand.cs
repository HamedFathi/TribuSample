using HamedStack.CQRS;

namespace Tributech.Application.Update
{
    public class UpdateSensorCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double UpperWarningLimit { get; set; }
        public double LowerWarningLimit { get; set; }
    }
}
