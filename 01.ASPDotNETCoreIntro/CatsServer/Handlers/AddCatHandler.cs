namespace CatsServer.Handlers
{
    using Data;
    using System;
    using Microsoft.AspNetCore.Http;
    using Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public class AddCatHandler : IHandler
    {
        public int Order => 2;

        public Func<HttpContext, bool> Condition => 
            context => context.Request.Path.Value == "/cat/add";

        public RequestDelegate RequestHandler => async ctx =>
        {
            if (ctx.Request.Method == HttpMethod.Get)
            {
                ctx.Response.Redirect("/cats-add-form.html");
            }
            else if (ctx.Request.Method == HttpMethod.Post)
            {
                var db = ctx.RequestServices.GetRequiredService<CatsDbContext>();
                var formData = ctx.Request.Form;
                int age = 0;
                int.TryParse(formData["Age"], out age);
                var cat = new Cat
                {
                    Name = formData["Name"],
                    Age = age,
                    Breed = formData["Breed"],
                    ImageUrl = formData["ImageUrl"]
                };

                try
                {
                    if (string.IsNullOrWhiteSpace(cat.Name) ||
                    string.IsNullOrWhiteSpace(cat.Breed) ||
                    string.IsNullOrWhiteSpace(cat.ImageUrl))
                    {
                        throw new InvalidOperationException("Invalid cat data!");
                    }

                    db.Cats.Add(cat);

                    await db.SaveChangesAsync();

                    ctx.Response.Redirect("/");
                }
                catch
                {
                    await ctx.Response.WriteAsync("<h2>Invalid cat data!</h2>");
                    await ctx.Response.WriteAsync(@"<a href=""/cat/add"">Back to the Form</a>");
                }
            }
        };
    }
}