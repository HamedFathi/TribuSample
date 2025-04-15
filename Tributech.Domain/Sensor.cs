using HamedStack.TheAggregateRoot;
using HamedStack.TheAggregateRoot.Abstractions;
using HamedStack.TheResult;
using Tributech.Domain.ValueObjects;

namespace Tributech.Domain
{
    public class Sensor : Entity<Guid>, IAggregateRoot
    {
        public Text Name { get; set; }
        public Text Location { get; set; }
        public CreationTime CreationTime { get; set; }
        public WarningLimit WarningLimit { get; set; }

        private Sensor()
        {
        }

        private Sensor(Text name, Text location, CreationTime creationTime, WarningLimit warningLimit)
        {
            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            CreationTime = creationTime;
            WarningLimit = warningLimit;
        }

        public static Result<Sensor> Create(string name, string location, DateTimeOffset creationTime, double lowerWarningLimit, double upperWarningLimit)
        {
            var nameValue = Text.Create(name);
            if (!nameValue.IsSuccess)
            {
                return Result<Sensor>.Failure(null, nameValue.Errors);
            }

            var locationValue = Text.Create(location);
            if (!locationValue.IsSuccess)
            {
                return Result<Sensor>.Failure(null, locationValue.Errors);
            }

            var creationTimeValue = CreationTime.Create(creationTime);
            if (!creationTimeValue.IsSuccess)
            {
                return Result<Sensor>.Failure(null, creationTimeValue.Errors);
            }

            var warningLimitValue = WarningLimit.Create(lowerWarningLimit, upperWarningLimit);
            if (!warningLimitValue.IsSuccess)
            {
                return Result<Sensor>.Failure(null, warningLimitValue.Errors);
            }

            return Result<Sensor>.Success(new Sensor(nameValue.Value!, locationValue.Value!, creationTimeValue.Value!, warningLimitValue.Value!));
        }
    }
}
