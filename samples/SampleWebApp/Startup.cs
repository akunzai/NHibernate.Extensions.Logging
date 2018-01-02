using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using SampleShared.Domain;
using SampleShared.Mapping;

namespace SampleWebApp
{
    public class Startup
    {
        public Startup(
            IHostingEnvironment env,
            Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.UseAsNHibernateLoggerProvider();
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
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
                    .CurrentSessionContext<AsyncLocalSessionContext>()
                    .ExposeConfiguration(cfg =>
                    {
                        new SchemaUpdate(cfg).Execute(true, true);
                    })
                    .Database(dbCfg)
                    .Diagnostics(d => d.OutputToConsole().Enable())
                    .BuildSessionFactory();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ISessionFactory sessionFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                using (var session = sessionFactory.OpenSession())
                {
                    CurrentSessionContext.Bind(session);
                    await next.Invoke();
                }
            });
            app.Run(async (context) =>
            {
                var session = sessionFactory.GetCurrentSession();
                var todos = session.QueryOver<Todo>().Where(x => x.Title == "Test").List();
                if (todos.Any())
                {
                    await context.Response.WriteAsync($"Found Todo: {todos.First().Id}");
                    return;
                }
                using (var tx = session.BeginTransaction())
                {
                    var entity = new Todo { Title = "Test" };
                    session.Save(entity);
                    await context.Response.WriteAsync($"Saved Todo: {entity.Id}");
                    tx.Commit();
                }
            });
        }
    }
}
