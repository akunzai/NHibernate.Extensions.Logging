using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace NHibernate.Extensions.Logging.Tests;

public class MicrosoftLoggerFactoryTests
{
    private readonly Microsoft.Extensions.Logging.ILoggerFactory _mockFactory;
    private readonly MicrosoftLoggerFactory _factory;

    public MicrosoftLoggerFactoryTests()
    {
        _mockFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
        _factory = new MicrosoftLoggerFactory(_mockFactory);
    }

    [Fact]
    public void LoggerFor_WithName_ReturnsMicrosoftLogger()
    {
        // Arrange
        var name = "TestLogger";
        var mockLogger = Substitute.For<ILogger>();
        _mockFactory.CreateLogger(name).Returns(mockLogger);

        // Act
        var logger = _factory.LoggerFor(name);

        // Assert
        Assert.IsType<MicrosoftLogger>(logger);
        _mockFactory.Received(1).CreateLogger(name);
    }

    [Fact]
    public void LoggerFor_WithType_ReturnsMicrosoftLogger()
    {
        // Arrange
        var type = typeof(MicrosoftLoggerFactoryTests);
        var mockLogger = Substitute.For<ILogger>();
        _mockFactory.CreateLogger(type.FullName).Returns(mockLogger);

        // Act
        var logger = _factory.LoggerFor(type);

        // Assert
        Assert.IsType<MicrosoftLogger>(logger);
    }
}
