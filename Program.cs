using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using webapi.Contracts;
using webapi.Data;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// add DBContext here later
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IPersonService, PersonService>();

// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo {
//         Title = "PizzaStore API",
//         Description = "Making the Pizzas you love",
//         Version = "v1" });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// app.UseRewriter(new RewriteOptions().AddRedirect("history", "about"));
// app.MapGet("/", () => "Hello World!");
// app.MapGet("/welcome", (IPersonService personService) => personService.GetWelcomeMessage());
// app.MapGet("/persons", (IPersonService personService) =>
// {
//     return $"Hello, {personService.GetPersonName()}!";
// });


app.UseRouting();
app.UsePathBase("/api");

app.UseHttpsRedirection();

app.UseAuthorization();

// app.UseAntiforgery(); // csrf

app.MapControllers();

// app.Use(async (context, next) =>
// {
//     await context.Response.WriteAsync("Hello from middleware 1. Passing to the next middleware!\r\n");
//
//     // Call the next middleware in the pipeline
//     await next.Invoke();
//
//     await context.Response.WriteAsync("Hello from middleware 1 again!\r\n");
// });
//
// app.Run(async context =>
// {
//     await context.Response.WriteAsync("Hello from middleware 2!\r\n");
// });

// app.Use(async (context, next) =>
// {
//     Console.WriteLine($"{context.Request.Method} {context.Request.Path} {context.Response.StatusCode}");
//     await next(); 
// });

app.Run();
// app.Run("http://localhost:2000");
