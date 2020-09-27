using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddDistributedMemoryCache();

            services.ConfigureDistributedCacheKeySpace("teste");

            services.AddSimpleCache<IEnumerable<WeatherForecast>>();

            services.AddSimpleCache<Guid, WeatherForecast>();
            services.AddSimpleCache<Guid, WeatherForecast>("custom-1", opt => opt
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(10))
                .WithKeyPrefix("teste1"));
            services.AddSimpleCache<Guid, WeatherForecast>("custom-2", opt => opt
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(10))
                .WithKeyPrefix("teste2"));

            services.AddSimpleCache<Guid, WeatherForecast>(opt => opt.WithKeyPrefix("teste2"));
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
