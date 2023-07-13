using NHibernate;
using NHibernate.Extensions.Logging;

namespace Microsoft.Extensions.Logging;

public static class NHibernateLoggerProviderExtensions
{
    public static ILoggerFactory UseAsNHibernateLoggerProvider(this ILoggerFactory factory)
    {
        NHibernateLogger.SetLoggersFactory(new MicrosoftLoggerFactory(factory));
        return factory;
    }
}