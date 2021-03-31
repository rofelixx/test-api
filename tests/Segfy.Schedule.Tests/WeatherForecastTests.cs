using System;
using Xunit;
using Segfy.Schedule.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FluentAssertions;

namespace Segfy.Schedule.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void WeatherForecastController_Get_ShouldReturnRandomArray()
        {
            Mock<ILogger<WeatherForecastController>> mock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(mock.Object);

            var result = controller.Get();

            result.Should().NotBeNull();
        }
    }
}
