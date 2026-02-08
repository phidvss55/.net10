using webapi.Extensions;
using webapi.Middleware;
using webapi.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddOpenApi();

// Custom Service Registrations
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerServices();

// Razor Pages and Components
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseMiddleware<LoggerMiddleware>();

// Route Mapping
app.MapRazorPages();
app.MapRazorComponents<webapi.Components.App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
app.MapGameRoutes();

string hostUrl = builder.Configuration["AppSettings:HostUrl"] ?? "http://localhost:8080";
app.Run(hostUrl);

