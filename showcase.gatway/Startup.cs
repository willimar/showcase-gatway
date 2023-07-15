using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using showcase.gatway.Aggregators;
using showcase.gatway.Delegates;

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

            services.AddCors(delegate (CorsOptions options)
            {
                options.AddPolicy("_AllowAnyOrigins", delegate (CorsPolicyBuilder builder)
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
                options.AddPolicy("_OptionsPolicy", delegate (CorsPolicyBuilder builder)
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddSingleton<DelegateOption>(Configuration.GetSection(nameof(DelegateOption)).Get<DelegateOption>());

            services.AddOcelot(Configuration)
                .AddSingletonDefinedAggregator<SwaggerDetailAggregator>()
                .AddConsul()
                .AddDelegatingHandler<DelegateBase>()
                //.AddAdministration("/administration", "secret")
                ;
        }

        public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors("_AllowAnyOrigins");
            app.UseCors("_OptionsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot().GetAwaiter().GetResult();
        }
    }
}
