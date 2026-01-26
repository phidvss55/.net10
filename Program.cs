using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using webapi.Contracts;
using webapi.Data;
using webapi.Middleware;
using webapi.Models;
using webapi.Repository;
using webapi.Routes;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddValidation();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "PizzaStore API",
        Description = "Making the Pizzas you love",
        Version = "v1" 
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",   
        Scheme =  "bearer",
    });
});

// add DBContext here later
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = options.DefaultChallengeScheme = options.DefaultForbidScheme = options.DefaultScheme = options.DefaultSignInScheme = options
        .DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme; 
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience =  builder.Configuration["JwtSettings:Audience"],
        ValidateIssuerSigningKey =  true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])
        ),
    };
});

builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();

builder.Services.AddScoped<IFMPService, FMPService>();
builder.Services.AddHttpClient<IFMPService, FMPService>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

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
app.MapGet("/", () => "Hello World!");
// app.MapGet("/welcome", (IPersonService personService) => personService.GetWelcomeMessage());
// app.MapGet("/persons", (IPersonService personService) =>
// {
//     return $"Hello, {personService.GetPersonName()}!";
// });

app.UseRouting();
// app.UsePathBase("/api");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
// app.UseAntiforgery(); // csrf
app.MapControllers();

app.MapGameRoutes();

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
//
// app.Use(async (context, next) =>
// {
//     Console.WriteLine($"{context.Request.Method} {context.Request.Path} {context.Response.StatusCode}");
//     await next(); 
// });

app.UseMiddleware<LoggerMiddleware>();

string hostUrl = builder.Configuration["AppSettings:HostUrl"] ?? "http://localhost:8001";
app.Run(hostUrl);
