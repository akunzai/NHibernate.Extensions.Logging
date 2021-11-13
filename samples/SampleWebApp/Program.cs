using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using SampleShared.Domain;
using SampleShared.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(_ =>
{
    var dbCfg = SQLiteConfiguration.Standard
            .ConnectionString(builder.Configuration.GetConnectionString("System.Data.SQLite"))
            // https://sqlite.org/isolation.html
            .IsolationLevel(System.Data.IsolationLevel.Serializable)
            .ShowSql()
            .FormatSql();
    return Fluently.Configure()
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TodoMap>())
        .CurrentSessionContext<AsyncLocalSessionContext>()
        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
        .Database(dbCfg)
        .Diagnostics(d => d.OutputToConsole().Enable())
        .BuildSessionFactory();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    var loggerFactory = app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
    loggerFactory.UseAsNHibernateLoggerProvider();
}

app.Use(async (context, next) =>
{
    var sessionFactory = context.RequestServices.GetRequiredService<ISessionFactory>();
    using var session = sessionFactory.OpenSession();
    CurrentSessionContext.Bind(session);
    await next.Invoke().ConfigureAwait(false);
});

app.Run(async context =>
{
    var sessionFactory = context.RequestServices.GetRequiredService<ISessionFactory>();
    var session = sessionFactory.GetCurrentSession();
    var entities = session.QueryOver<Todo>().Where(x => x.Title == "Test").List();
    await context.Response.WriteAsync($"<h1>Found Entities: {entities.Count}</h1>").ConfigureAwait(false);
    using var tx = session.BeginTransaction();
    var entity = new Todo { Title = "Test" };
    session.Save(entity);
    await context.Response.WriteAsync($"<h2>Saved Entity: {entity.Id}</h2>").ConfigureAwait(false);
    tx.Commit();
});

app.Run();
