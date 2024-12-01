using FluentValidation;
using FluentValidation.Results;
using HamedStack.TheRepository;
using Moq;
using Tributech.Application.Create;
using Tributech.Domain;

namespace Tributech.UnitTests
{
    public class SensorUnitTest
    {
        [Fact]
        public async Task ShouldReturnSuccessWhenSensorIsCreated()
        {
            var mockRepo = new Mock<IRepository<Sensor>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var sensor = Sensor.Create("test","test_location", DateTimeOffset.Now, 10, 20);

            mockRepo.Setup(repository => repository.AddAsync(It.IsAny<Sensor>(), CancellationToken.None))
                .ReturnsAsync(sensor);

            mockRepo.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            var mockValidator = new Mock<IValidator<CreateSensorCommand>>();
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<CreateSensorCommand>(), CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var handler = new CreateSensorCommandHandler(mockRepo.Object, mockValidator.Object);

            var command = new CreateSensorCommand()
            {
                Location = "test_location",
                LowerWarningLimit = 10,
                Name = "test",
                UpperWarningLimit = 20
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);

            mockValidator.Verify(v => v.ValidateAsync(It.IsAny<CreateSensorCommand>(), CancellationToken.None), Times.Once);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Sensor>(), CancellationToken.None), Times.Once);

        }
    }
}