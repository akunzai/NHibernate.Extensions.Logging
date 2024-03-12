# NHibernate.Extensions.Logging

[![Build Status][build-badge]][build] [![Code Coverage][codecov-badge]][codecov]
[![NuGet version][nuget-badge]][nuget]

[build]: https://github.com/akunzai/NHibernate.Extensions.Logging/actions/workflows/build.yml
[build-badge]: https://github.com/akunzai/NHibernate.Extensions.Logging/actions/workflows/build.yml/badge.svg
[codecov]: https://codecov.io/gh/akunzai/NHibernate.Extensions.Logging
[codecov-badge]: https://codecov.io/gh/akunzai/NHibernate.Extensions.Logging/branch/main/graph/badge.svg?token=OQLZMRDOTM
[nuget]: https://www.nuget.org/packages/NHibernate.Extensions.Logging/
[nuget-badge]: https://img.shields.io/nuget/v/NHibernate.Extensions.Logging.svg?style=flat-square

Use Microsoft.Extensions.Logging as NHibernate logging provider

## Installation

```shell
dotnet add package NHibernate.Extensions.Logging
```

## Usage

Console App

```csharp
// ...
services.AddSingleton<ISessionFactory>(resolver =>
{
    var env = resolver.GetRequiredService<IHostEnvironment>();
    if (env.IsDevelopment())
    {
        var loggerFactory = resolver.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
        loggerFactory.UseAsNHibernateLoggerProvider();
    }
    // ...
    // return ISessionFactory implementation
});
```

ASP.NET Core

```csharp
public class Startup
{
    // ...
    public ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ISessionFactory>(resolver =>
        {
            var env = resolver.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                var loggerFactory = resolver.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
                loggerFactory.UseAsNHibernateLoggerProvider();
            }
            // ...
            // return ISessionFactory implementation
        });
        // ...
    }
}
```

ASP.NET Core Minimal APIs

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISessionFactory>(resolver =>
{
    var env = resolver.GetRequiredService<IWebHostEnvironment>();
    if (env.IsDevelopment())
    {
        var loggerFactory = resolver.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
        loggerFactory.UseAsNHibernateLoggerProvider();
    }
    // ...
    // return ISessionFactory implementation
});
// ...
```
