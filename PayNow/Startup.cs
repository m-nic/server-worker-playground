using Hangfire;
using Owin;
using System;

namespace PayNow;

public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseErrorPage();

        SetupHangfire(app);

        app.Run(context =>
        {
            context.Response.ContentType = "text/plain";

            if (context.Request.Path.Value == "/")
            {
                PaymentService.Pay();
            }

            return context.Response.WriteAsync("Hello, world.");
        });
    }

    private void SetupHangfire(IAppBuilder app)
    {
        GlobalConfiguration.Configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB; Database=HangfireTest; Integrated Security=True;");
    }
}
