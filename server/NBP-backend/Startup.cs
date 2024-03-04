using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBP_backend.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.SignalR;

using NBP_backend.Cache;

namespace NBP_backend
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NBP_backend", Version = "v1" });
            });

            services.AddSingleton<UserServices>();
            services.AddSingleton<ProductServices>();
            services.AddSingleton<MarketServices>();
            services.AddSingleton<CategoryServices>();
            services.AddSingleton<ManufacturerServices>();
            services.AddSingleton<ReviewServices>();
            services.AddSingleton<NotificationServices>();
            services.AddSingleton<OrderProductServices>();
            services.AddSingleton<DeliveryServices>();


            var client = new BoltGraphClient(new Uri("neo4j+s://ea17674b.databases.neo4j.io"), "neo4j", "PbWMDupdf6n1LrZRBjibXkoJZ05YffMXokUZTFwyRrk");
            client.ConnectAsync();
            services.AddSingleton<IGraphClient>(client);

            services.AddTransient<ICacheProvider, CacheProvider>();
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });
            services.AddSignalR();

            //
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder.WithOrigins(new string[]
                    {
                        "http://localhost:8080",
                        "https://localhost:8080",
                        "http://127.0.0.1:8080",
                        "https://127.0.0.1:8080",
                        "http://localhost:5500",
                        "https://localhost:5500",
                        "http://127.0.0.1:5500",
                        "https://127.0.0.1:5500",
                        "https://localhost:5001",
                        "https://127.0.0.1:5001",
                        "http://127.0.0.1:5001",
                        "https://127.0.0.1:5001",
                        "http://localhost:3000",
                        "https://localhost:3000",
                        "https://localhost:5001",
                        "http://localhost:5001",






                        "http://localhost:3000",
                        "https://localhost:3000",
                        "http://127.0.0.1:3000"


                    })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    
                });

            });
         }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NBP_backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CORS");

            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ProductHub>("/producthub");

            });
         
        }
    }
}
