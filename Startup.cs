using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;/*
using Microsoft.Extensions.Options;*/

namespace Platform
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.Configure<MessageOptions>(options => {
                options.CityName = "Albany";
            });*/
            services.Configure<RouteOptions>(opts => {
                opts.ConstraintMap.Add("countryName",
                typeof(CountryRouteConstraint));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, IOptions<MessageOptions> msgOptions*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<Population>();
            //app.UseMiddleware<Capital>();

            app.UseRouting();

            app.Use(async (context, next) => {
                Endpoint end = context.GetEndpoint();
                if (end != null)
                {
                    await context.Response
                    .WriteAsync($"{end.DisplayName} Selected \n");
                }
                else
                {
                    await context.Response.WriteAsync("No Endpoint Selected \n");
                }
                await next();
            });

            app.UseEndpoints(endpoints => {/*

                endpoints.MapGet("files/{filename}.{ext}", async context => {
                    await context.Response.WriteAsync("Request Was Routed\n");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response
                        .WriteAsync($"{kvp.Key}: {kvp.Value}\n");
                    }
                });
                endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);
                endpoints.MapGet("size/{city}", Population.Endpoint).WithMetadata(new RouteNameMetadata("population"));
                endpoints.MapFallback(async context => {
                    await context.Response.WriteAsync("Routed to fallback endpoint");
                });*/
                endpoints.Map("{number:int}", async context => {
                    await context.Response.WriteAsync("Routed to the int endpoint");
                })
                .WithDisplayName("Int Endpoint")
                .Add(b => ((RouteEndpointBuilder)b).Order = 1);
                endpoints.Map("{number:double}", async context => {
                    await context.Response
                    .WriteAsync("Routed to the double endpoint");
                })
                .WithDisplayName("Double Endpoint")
                .Add(b => ((RouteEndpointBuilder)b).Order = 2);
            });

            app.Run(async (context) => {
                await context.Response.WriteAsync("Terminal Middleware Reached");
            });

            /*app.UseMiddleware<LocationMiddleware>();*/

            /*app.Use(async (context, next) => {
                if (context.Request.Path == "/location")
                {
                    MessageOptions opts = msgOptions.Value;
                    await context.Response
                    .WriteAsync($"{opts.CityName}, {opts.CountryName}");
                }
                else
                {
                    await next();
                }
            });*/

            /*app.Use(async (context, next) => {
                await next();
                await context.Response
                .WriteAsync($"\nStatus Code: { context.Response.StatusCode}");
            });

            app.Use(async (context, next) => {
                if (context.Request.Path == "/short")
                {
                    await context.Response
                    .WriteAsync($"Request Short Circuited");
                }
                else
                {
                    await next();
                }
            });

            app.Use(async (context, next) => {
                if (context.Request.Method == HttpMethods.Get
                && context.Request.Query["custom"] == "true")
                {
                    await context.Response.WriteAsync("Custom Middleware \n");
                }
                await next();
            });

            app.Map("/branch", branch => {
                branch.Run(new QueryStringMiddleWare().Invoke);
                branch.Run(async (context) => {
                    await context.Response.WriteAsync($"Branch Middleware");
                });
            });

            app.MapWhen(context => context.Request.Query.Keys.Contains("mapped"),
               branch => {
                branch.Use(async (context, next) => {
                    await context.Response.WriteAsync($"MapWhen Middleware");
                });
            });

            app.UseMiddleware<QueryStringMiddleWare>();*/
        }
    }
}
