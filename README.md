# NHibernate.Extensions.Logging

[![Build Status][ci-badge]][ci] [![Code Coverage][codecov-badge]][codecov]
[![NuGet version][nuget-badge]][nuget]

[ci]: https://github.com/akunzai/NHibernate.Extensions.Logging/actions?query=workflow%3ACI
[ci-badge]: https://github.com/akunzai/NHibernate.Extensions.Logging/workflows/CI/badge.svg
[codecov]: https://codecov.io/gh/akunzai/NHibernate.Extensions.Logging
[codecov-badge]: https://codecov.io/gh/akunzai/NHibernate.Extensions.Logging/branch/main/graph/badge.svg?token=OQLZMRDOTM
[nuget]: https://www.nuget.org/packages/NHibernate.Extensions.Logging/
[nuget-badge]: https://img.shields.io/nuget/v/NHibernate.Extensions.Logging.svg?style=flat-square

Use Microsoft.Extensions.Logging as NHibernate logging provider

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
var env = services.GetService<IHostEnvironment>();
if (env.IsDevelopment())
{
    var loggerFactory = services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
    loggerFactory.UseAsNHibernateLoggerProvider();
}
```

ASP.NET Core

```csharp
public class Startup
{
    ...
    public Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            loggerFactory.UseAsNHibernateLoggerProvider();
        }
        ...
    }
}
```

ASP.NET Core Minimal APIs

```csharp
...
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    var loggerFactory = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
    loggerFactory.UseAsNHibernateLoggerProvider();
}
```
