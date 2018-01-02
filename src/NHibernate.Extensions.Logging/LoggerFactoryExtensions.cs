using NHibernate;
using NHibernate.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory UseAsNHibernateLoggerProvider(this ILoggerFactory factory)
        {
            LoggerProvider.SetLoggersFactory(new MicrosoftLoggerFactory(factory));
            return factory;
        }
    }
}
