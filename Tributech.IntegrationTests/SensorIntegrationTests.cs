using FluentAssertions;
using HamedStack.WebIntegrationTest;
using Tributech.Application.Create;
using Tributech.Infrastructure;
using Tributech.Presentation;

namespace Tributech.IntegrationTests;

public class SensorIntegrationTests : WebIntegrationTestBase<Program,TributechDbContext>
{
    public SensorIntegrationTests() : base(new IntegrationTestWebAppFactory())
    { }

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