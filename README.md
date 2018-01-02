# NHibernate.Extensions.Logging

use Microsoft.Extensions.Logging as NHibernate logging provider

[![NuGet version](https://img.shields.io/nuget/v/NHibernate.Extensions.Logging.svg?style=flat-square)](https://www.nuget.org/packages/NHibernate.Extensions.Logging/)

## Usage

Console App

```csharp
class Program()
{
	static void Main(string[] args)
	{
		var loggerFactory = new LoggerFactory()
		.AddDebug()
		.UseAsNHibernateLoggerProvider();
	}
}
```

ASP.NET Core 1.x

```csharp
public class Startup
{
	public void Configure(
        IApplicationBuilder app,
        IHostingEnvironment env,
        ILoggerFactory loggerFactory)
	{
		if (env.IsDevelopment())
		{
			loggerFactory.UseAsNHibernateLoggerProvider();
		}
	}
}
```

ASP.NET Core 2.x

```csharp
public class Startup
{
	public Startup(
        IHostingEnvironment env,
        ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            loggerFactory.UseAsNHibernateLoggerProvider();
        }
    }
}
```

## [NuGet Package](https://www.nuget.org/packages/NHibernate.Extensions.Logging)
## [Release Notes](https://github.com/akunzai/NHibernate.Extensions.Logging/releases)
## [License](LICENSE.md)