using Microsoft.Extensions.Logging;
using NHibernate;
using Xunit;
using NSubstitute;

namespace NHibernate.Extensions.Logging.Tests;

public class NHibernateLoggerProviderExtensionsTests
{
    [Fact]
    public void UseAsNHibernateLoggerProvider_SetsLoggerFactory()
    {
        // Arrange
        var mockFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();

        // Act
        mockFactory.UseAsNHibernateLoggerProvider();

        // Assert
        var logger = NHibernateLogger.For("test");
        Assert.NotNull(logger);
    }
}
