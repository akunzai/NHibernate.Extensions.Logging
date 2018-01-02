using System;
using System.IO;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SampleShared.Domain;
using SampleShared.Mapping;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var sessionFactory = serviceProvider.GetService<ISessionFactory>();
            using (var session = sessionFactory.OpenSession())
            {
                var todos = session.QueryOver<Todo>().Where(x => x.Title == "Test").List();
                if (todos.Any())
                {
                    Console.WriteLine($"Found Todo: {todos.First().Id}");
                    return;
                }
                using (var tx = session.BeginTransaction())
                {
                    var entity = new Todo { Title = "Test" };
                    session.Save(entity);
                    Console.WriteLine($"Saved Todo: {entity.Id}");
                    tx.Commit();
                }
            }
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build());
            var loggerFactory = new LoggerFactory()
                .AddDebug()
                .UseAsNHibernateLoggerProvider();
            services.AddSingleton(loggerFactory);
            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var dbCfg = SQLiteConfiguration.Standard
                    .ConnectionString(configuration.GetConnectionString("System.Data.SQLite"))
                    // https://sqlite.org/isolation.html
                    .IsolationLevel(System.Data.IsolationLevel.Serializable)
                    .ShowSql()
                    .FormatSql();
                return Fluently.Configure()
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TodoMap>())
                    .ExposeConfiguration(cfg =>
                    {
                        new SchemaUpdate(cfg).Execute(true, true);
                    })
                    .Database(dbCfg)
                    .Diagnostics(d => d.OutputToConsole().Enable())
                    .BuildSessionFactory();
            });
        }
    }
}
