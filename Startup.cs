using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Platform.Models;
using Platform.Services;
using System;
using System.Threading.Tasks;

namespace Platform
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        private IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedSqlServerCache(it => {
                it.ConnectionString = Configuration["ConnectionStrings:CacheConnection"];
                it.SchemaName = "dbo";
                it.TableName = "DataCache";
            });
            services.AddResponseCaching();
            services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
            services.AddDbContext<CalculationContext>(it => {
                it.UseSqlServer(Configuration["ConnectionStrings:CalcConnection"]);
            });
            services.AddTransient<SeedData>();
        }
        public void Configure(IApplicationBuilder app,
IHostApplicationLifetime lifetime, IWebHostEnvironment env,
SeedData seedData)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            if (env.IsProduction()) {
                app.UseHsts();
            }
            app.UseExceptionHandler("/error.html");
            app.UseStaticFiles();
            app.UseStatusCodePages("text/html", Responses.DefaultResponse);

            app.Use(async (context, next) => {
                if (context.Request.Path == "/error")
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await Task.CompletedTask;
                }
                else
                {
                    await next();
                }
            });
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapEndpoint<SumEndpoint>("/sum/{count:int=1000000000}");
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            bool cmdLineInit = (Configuration["INITDB"] ?? "false") == "true";
            if (env.IsDevelopment() || cmdLineInit)
            {
                seedData.SeedDatabase();
                if (cmdLineInit)
                {
                    lifetime.StopApplication();
                }
            }

        }
    }
}