using Common;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
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

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var activator = new DiHangfireActivator(serviceProvider);

        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseActivator(activator)
            .UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB; Database=HangfireTest; Integrated Security=True;");
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
}