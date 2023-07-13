using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using showcase.gatway.Aggregators;
using showcase.gatway.Delegates;
using showcase.gatway.Delegates.Showcase.Authenticate;

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
                .AddConsul()
                .AddDelegatingHandler<SampleHandler>()
                .AddDelegatingHandler<RegisterAccountHandler>()
                //.AddAdministration("/administration", "secret")
                ;
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
