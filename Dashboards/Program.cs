using Dapper;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using StackExchange.Profiling.Storage;
using MiniProfilerSqlStorage = StackExchange.Profiling.Storage.SqlServerStorage;

namespace Dashboards
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            ConfigureHangfire(builder);
            ConfigureMiniProfiler(builder);

            var app = builder.Build();

            app.MapDefaultEndpoints();

            app.UseMiniProfiler();
            app.UseHangfireDashboard("");
            app.Run();
        }

        private static void ConfigureMiniProfiler(WebApplicationBuilder builder)
        {
            MiniProfilerSqlStorage sqlServerStorage = new("Server=(localdb)\\MSSQLLocalDB; Database=MiniProfiler; Integrated Security=True;");

            if (sqlServerStorage is DatabaseStorageBase dbs && sqlServerStorage is IDatabaseStorageConnectable dbsc)
            {
                using var conn = dbsc.GetConnection();
                foreach (var script in dbs.TableCreationScripts)
                {
                    try
                    {
                        conn.Execute(script);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error creating MiniProfiler table: " + ex.Message);
                    }
                }
            }

            builder.Services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = "/profiler";
                    options.Storage = sqlServerStorage;

                    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                    options.ResultsAuthorize = request => true;
                    options.ResultsListAuthorize = request => true;

                    options.ShouldProfile = request => false;

                    options.TrackConnectionOpenClose = false;

                    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;

                    options.EnableMvcFilterProfiling = false;
                    options.EnableMvcViewProfiling = false;
                    options.OnInternalError = e => Console.Error.WriteLine("MiniProfiler Err: " + e);
                });
        }

        private static void ConfigureHangfire(WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(config =>
                config
                .UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB; Database=HangfireTest; Integrated Security=True;")
            );


            DashboardRoutes.Routes.AddRazorPage("/mini-profiler", x => new MiniProfilerPage());
            NavigationMenu.Items.Add(page => new MenuItem("MiniProfiler", page.Url.To("/mini-profiler"))
            {
                Active = page.RequestPath.StartsWith("/mini-profiler")
            });
        }
    }

    public class MiniProfilerPage : RazorPage
    {
        public override void Execute()
        {
            Layout = new LayoutPage("MiniProfiler");
            WriteLiteral("<iframe src=\"/profiler/results-index\" style=\"width: 100%;height=80vh; border:0\" />");
        }

    }
}
