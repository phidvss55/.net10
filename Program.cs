using webapi;
using webapi.Extensions;
using webapi.Middleware;
using webapi.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
    {
        options.Conventions.Add(new ApiPrefixConvention());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// Custom Service Registrations
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Razor Pages and Views
builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Views/Pages";
});
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
// app.UseMiddleware<AuthenticationMiddleware>("Test");

// Route Mapping
app.MapRazorPages();
app.MapRazorComponents<webapi.Views.App>().AddInteractiveServerRenderMode();

// api controller mapping
app.MapControllers();
app.MapGameRoutes();

var hostUrl = builder.Configuration["AppSettings:HostUrl"];

if (string.IsNullOrWhiteSpace(hostUrl))
{
    await app.RunAsync();
}
else
{
    await app.RunAsync(hostUrl);
}

