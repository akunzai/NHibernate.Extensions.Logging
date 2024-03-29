using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SampleShared.Domain;
using SampleShared.Mapping;

var services = Host.CreateDefaultBuilder()
.ConfigureServices((context, services) =>
{
    services.AddSingleton(resolver =>
    {
        var env = resolver.GetRequiredService<IHostEnvironment>();
        if (env.IsDevelopment())
        {
            var loggerFactory = resolver.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
            loggerFactory.UseAsNHibernateLoggerProvider();
        }
        var dbCfg = SQLiteConfiguration.Standard
            .ConnectionString(context.Configuration.GetConnectionString("System.Data.SQLite"))
            // https://sqlite.org/isolation.html
            .IsolationLevel(System.Data.IsolationLevel.Serializable)
            .ShowSql()
            .FormatSql();
        return Fluently.Configure()
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TodoMap>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
            .Database(dbCfg)
            .Diagnostics(d => d.OutputToConsole().Enable())
            .BuildSessionFactory();
    });
}).Build().Services;

var sessionFactory = services.GetRequiredService<ISessionFactory>();
using (var session = sessionFactory.OpenSession())
{
    var entities = session.QueryOver<Todo>().Where(x => x.Title == "Test").List();
    Console.WriteLine($"Found Entities: {entities.Count}");
    using var tx = session.BeginTransaction();
    var entity = new Todo { Title = "Test" };
    session.Save(entity);
    Console.WriteLine($"Saved Entity: {entity.Id}");
    tx.Commit();
}
Console.WriteLine("Press the ANY key to exit...");
Console.ReadKey();
