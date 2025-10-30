var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Dashboards>("dashboards");
builder.AddProject<Projects.PayNow>("paynow");
builder.AddProject<Projects.PayNowWorker>("paynowworker");

builder.Build().Run();
