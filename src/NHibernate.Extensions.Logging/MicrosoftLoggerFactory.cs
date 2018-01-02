using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.Logging
{
    /// <summary>
    /// Implementation of the <see cref="NHibernate.ILoggerFactory"/> interface 
    /// to allow the usage of Microsoft.Extensions.Logging with the NHibernate 
    /// logging infrastructure.
    /// <seealso cref="MicrosoftLogger"/>
    /// <example>
    /// To use this logger factory with NHibernate add the following code to your code:
    /// <code>
    /// loggerFactory.UseAsNHibernateLoggerProvider();
    /// </code>
    /// </example>
    /// </summary>
    public class MicrosoftLoggerFactory : ILoggerFactory
    {
        private readonly Microsoft.Extensions.Logging.ILoggerFactory _factory;

        public MicrosoftLoggerFactory(Microsoft.Extensions.Logging.ILoggerFactory factory)
        {
            _factory = factory;
        }

		#region ILoggerFactory

		public IInternalLogger LoggerFor(string keyName)
	    {
		    return new MicrosoftLogger(_factory.CreateLogger(keyName));
	    }

	    public IInternalLogger LoggerFor(System.Type type)
	    {
		    return new MicrosoftLogger(_factory.CreateLogger(type));
	    }

	    #endregion
    }
}
