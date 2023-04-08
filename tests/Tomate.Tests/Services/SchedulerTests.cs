using NSubstitute;
using Tomate.Models;
using Tomate.Services;
using Tomate.Services.Abstractions;

namespace Tomate.Tests.Services;

public class SchedulerTests
{
    private readonly Scheduler cut;
    private readonly IDelay delay = Substitute.For<IDelay>();
    public SchedulerTests()
    {
        this.cut = new Scheduler(this.delay);
    }
    
    [Fact]
    public async Task ScheduleMinutes_ShouldYieldMinutes()
    {
        // Arrange
        var minutes = 5;
        var expected = new Minutes[] { 5, 4, 3, 2, 1 };
        
        // Act
        var yielded = this.cut.ScheduleMinutes(minutes);

        var index = 0;
        await foreach (var value in yielded)
        {
            value.Should().Be(expected[index]);

            index++;
        }
        
    }
}