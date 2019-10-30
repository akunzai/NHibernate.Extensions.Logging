# NHibernate.Extensions.Logging

use Microsoft.Extensions.Logging as NHibernate logging provider

[![NuGet version](https://img.shields.io/nuget/v/NHibernate.Extensions.Logging.svg?style=flat-square)](https://www.nuget.org/packages/NHibernate.Extensions.Logging/)
[![Build status](https://ci.appveyor.com/api/projects/status/m76t1k6o82g494s3?svg=true)](https://ci.appveyor.com/project/akunzai/nhibernate-extensions-logging)

## Installation

For NHibernate >= 5.1.0

```shell
# Package Manager
Install-Package NHibernate.Extensions.Logging
# .NET CLI
dotnet add package NHibernate.Extensions.Logging
```

For NHibernate < 5.1.0

```shell
# Package Manager
Install-Package NHibernate.Extensions.Logging -Version 1.1.1
# .NET CLI
dotnet add package NHibernate.Extensions.Logging -Version 1.1.1
```

## Usage

Console App

```csharp
...
class Program()
{
    static void Main(string[] args)
    {
        ...
        var loggerFactory = services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
        loggerFactory.UseAsNHibernateLoggerProvider();
    }

    private static IServiceProvider ConfigureServices(IServiceCollection services)
    {
        ...
    }
}
```

ASP.NET Core

```csharp
...
public class Startup
{
    public Startup(ILoggerFactory loggerFactory)
    {
        loggerFactory.UseAsNHibernateLoggerProvider();
    }
}
```
