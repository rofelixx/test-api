using System;
using Xunit;
using Segfy.Schedule.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FluentAssertions;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Options;
using Segfy.Schedule.Model.Configuration;

namespace Segfy.Schedule.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void WeatherForecastController_Get_ShouldReturnRandomArray()
        {
            Mock<ILogger<WeatherForecastController>> mock = new Mock<ILogger<WeatherForecastController>>();
            Mock<IAmazonDynamoDB> mockdb = new Mock<IAmazonDynamoDB>();
            Mock<IOptions<AppConfiguration>> mockopt = new Mock<IOptions<AppConfiguration>>();
            var controller = new WeatherForecastController(mock.Object, mockopt.Object, mockdb.Object);

            var result = controller.Get();

            result.Should().NotBeNull();
        }
    }
}
