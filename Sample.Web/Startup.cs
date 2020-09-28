using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DateTime = System.DateTime;

namespace Sample.Web
{
    public class Startup
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

            // Add in memory cache for testing.
            services.AddDistributedMemoryCache();

            // Add logging to all IDistributedCache operations.
            services.AddDistributedCacheLogging();

            // Configure global key space partitioning to avoid key collisions with other microservices.
            services.ConfigureDistributedCacheKeySpace("weather-forecast-service");

            // Add SimpleCache for the controller response.
            services.AddSimpleCache<IEnumerable<WeatherForecast>>(opt => opt
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromSeconds(5))
            );

            // Add SimpleCache for individual forecasts.
            services.AddSimpleCache<DateTime, WeatherForecast>(opt => opt
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromSeconds(15))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
