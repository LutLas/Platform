using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace Platform
{
    public class Capital
    {

        /*private RequestDelegate next;
        public Capital() { }
        public Capital(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }*/
        public static async Task Endpoint(HttpContext context)
        {
            /*string[] parts = context.Request.Path.ToString()
            .Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 && parts[0] == "capital")
            {*/
                string capital = null;
                string country = context.Request.RouteValues["country"] as string;
                switch ((country ?? "").ToLower())
                {
                    case "uk":
                        capital = "London";
                        break;
                    case "france":
                        capital = "Paris";
                        break;
                    case "monaco":
                    LinkGenerator generator = context.RequestServices.GetService<LinkGenerator>();
                    string url = generator.GetPathByRouteValues(context,"population", new { city = country });
                    context.Response.Redirect(url);
                        /*context.Response.Redirect($"/population/{country}");*/
                        return;
                }
                if (capital != null)
                {
                    await context.Response
                    .WriteAsync($"{capital} is the capital of {country}");
                    /*return;*/
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                /*}
                if (next != null)
                {
                    await next(context);
                }*/
            }
        }
}
