using CommonContracts;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using PayNowWorker.Hangfire;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using System;
using System.ComponentModel;

namespace PayNowWorker;
internal class Program
{
    static void Main(string[] args)
    {
        var a = new Startup();
        a.Configuration();
    }
}

public class Startup
{
    private void ConfigureHangfire()
    {
        ServiceCollection services = new();

        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(typeof(Startup).Assembly);
        });
        services.AddTransient<IScheduler, HangFireScheduler>();

        ConfigureMiniProfiler(services);

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var activator = new DiHangfireActivator(serviceProvider);

        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseActivator(activator)
            .UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB; Database=HangfireTest; Integrated Security=True;");


        GlobalConfiguration.Configuration.UseFilter(new MiniProfilerAttribute());
        GlobalConfiguration.Configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 3 });
    }

    public void Configuration()
    {
        ConfigureHangfire();

        //BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

        using (var server = new BackgroundJobServer())
        {
            Console.WriteLine("Hangfire Server started. Press any key to exit...");
            Console.ReadKey();
        }
    }

    private static void ConfigureMiniProfiler(IServiceCollection Services)
    {

        var options = new MiniProfilerOptions();
        var sqlServerStorage = new SqlServerStorage("Server=(localdb)\\MSSQLLocalDB; Database=MiniProfiler; Integrated Security=True;");
        options.Storage = sqlServerStorage;
        options.TrackConnectionOpenClose = false;

        options.OnInternalError = e => Console.Error.WriteLine("MiniProfiler Err: " + e);

        MiniProfiler.Configure(options);
    }
}