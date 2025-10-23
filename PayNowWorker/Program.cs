using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB; Database=HangfireTest; Integrated Security=True;");
    }

    public void Configuration()
    {
        ConfigureHangfire();

        BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

        using (var server = new BackgroundJobServer())
        {
            Console.ReadLine();
        }
    }
}