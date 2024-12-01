using HamedStack.CQRS;

namespace Tributech.Application.SelectAll;

public class GetSensorsQuery : IQuery<IEnumerable<SensorPoco>>
{
}