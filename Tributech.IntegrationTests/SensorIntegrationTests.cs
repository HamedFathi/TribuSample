using FluentAssertions;
using Tributech.Application.Create;

namespace Tributech.IntegrationTests;

public class SensorIntegrationTests : WebIntegrationTestBase
{
    public SensorIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenSensorIsCreated()
    {
        var output = await Dispatcher.Send(new CreateSensorCommand(){
            Name = "test",
            UpperWarningLimit = 20,
            LowerWarningLimit = 10,
            Location = "test_location"
        });

        output.IsSuccess.Should().BeTrue();
        output.Value.Should().NotBeEmpty();
    }
}