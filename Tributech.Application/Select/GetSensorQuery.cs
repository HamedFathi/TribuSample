using HamedStack.CQRS;

namespace Tributech.Application.Select
{
    public class GetSensorQuery : IQuery<SensorPoco>
    {
        public Guid Id { get; set; }
    }
}
