namespace CatsServer.Handlers
{
    using Infrastructure;
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using CatsServer.Data;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;

    public class HomeHandler : IHandler
    {
        public int Order => 1;

        public Func<HttpContext, bool> Condition => 
            context => context.Request.Path.Value == "/" && 
            context.Request.Method == HttpMethod.Get;

        public RequestDelegate RequestHandler => async ctx =>
        {
            var env = ctx.RequestServices.GetRequiredService<IHostingEnvironment>();
            await ctx.Response.WriteAsync($"<h1>{env.ApplicationName}</h1>");

            var db = ctx.RequestServices.GetRequiredService<CatsDbContext>();
            var catData = db.Cats
            .Select(c => new
            {
                c.Id,
                c.Name
            })
            .ToList();

            await ctx.Response.WriteAsync("<ul>");

            foreach (var cat in catData)
            {
                await ctx.Response.WriteAsync($@"<li><a href=""/cats/{cat.Id}"">{cat.Name}</a></li>");
            }

            await ctx.Response.WriteAsync("</ul>");
            await ctx.Response.WriteAsync(@"
                            <form action=""/cat/add"">
                                <input type=""submit"" value=""Add Cat"" />
                            </form>");
        };
    }
}