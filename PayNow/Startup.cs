using Owin;

namespace PayNow;

public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseErrorPage();

        app.Run(context =>
        {
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Hello, world.");
        });
    }
}