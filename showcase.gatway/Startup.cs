using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Runtime.CompilerServices;
using showcase.gatway.Aggregators;
using Ocelot.Provider.Consul;

namespace ShowCase.Broker
{
    public static class Startup
    {
        public static IConfiguration? Configuration { get; private set; }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddOcelot(Configuration)
                .AddSingletonDefinedAggregator<SwaggerDetailAggregator>()
                .AddConsul();
        }

        public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot().GetAwaiter().GetResult();
        }
    }
}
