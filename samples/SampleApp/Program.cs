using System;
using System.IO;
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
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Production";
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();
            var services = ConfigureServices(new ServiceCollection());
            var loggerFactory = services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
            loggerFactory.UseAsNHibernateLoggerProvider();
            var sessionFactory = services.GetService<ISessionFactory>();
            using (var session = sessionFactory.OpenSession())
            {
                var entities = session.QueryOver<Todo>().Where(x => x.Title == "Test").List();
                Console.WriteLine($"Found Entities: {entities.Count}");
                using (var tx = session.BeginTransaction())
                {
                    var entity = new Todo { Title = "Test" };
                    session.Save(entity);
                    Console.WriteLine($"Saved Entity: {entity.Id}");
                    tx.Commit();
                }
            }
            Console.WriteLine("Press the ANY key to exit...");
            Console.ReadKey();
        }

        private static IConfiguration Configuration { get; set; }

        private static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddLogging(logging =>
            {
                logging.AddConfiguration(Configuration.GetSection("Logging"));
                logging.AddDebug();
            });
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
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                    .Database(dbCfg)
                    .Diagnostics(d => d.OutputToConsole().Enable())
                    .BuildSessionFactory();
            });
            return services.BuildServiceProvider();
        }
    }
}
