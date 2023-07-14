using Ocelot.DependencyInjection;
using ShowCase.Broker;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddJsonFile("./Configurations/ocelot.json", optional: false, reloadOnChange: true)
            .AddJsonFile("./Configurations/showcase-autenticate.json", optional: false, reloadOnChange: true)
            .AddJsonFile("./Configurations/showcase-domain.json", optional: false, reloadOnChange: true)
            .AddOcelot("./Configurations/", hostingContext.HostingEnvironment)
            .AddEnvironmentVariables();
    });

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.Configure(app.Environment);

app.Run();