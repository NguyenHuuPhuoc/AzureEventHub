using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReceiveEvent.Features.User;
using ReceiveEvent.Features.User.Configuration;

[assembly: FunctionsStartup(typeof(ReceiveEvent.Startup))]

namespace ReceiveEvent
{
    public class Startup : FunctionsStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            AddReceiveEventHub(builder);
        }

        private static void AddReceiveEventHub(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<UserConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("UserConfig").Bind(settings);
                });
            builder.Services.AddScoped<IUserReceiveEvent, UserReceiveEvent>();
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}