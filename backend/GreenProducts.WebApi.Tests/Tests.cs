using Shouldly;
using Xunit;

namespace GreenProducts.WebApi.Tests;

[Collection(nameof(HostFixtureCollection))]
public class Tests(HostFixture hostFixture)
{
    [Fact]
    public void Application_CanStart()
    {
        hostFixture.WebApplication.Lifetime.ApplicationStarted.IsCancellationRequested.ShouldBeTrue("App should be started");
        hostFixture.WebApplication.Lifetime.ApplicationStopped.IsCancellationRequested.ShouldBeFalse("App should not be stopped");
    }
}