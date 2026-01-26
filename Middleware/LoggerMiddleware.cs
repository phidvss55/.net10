namespace webapi.Middleware;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    
    public LoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request
        Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");
        
        // Call the next middleware in the pipeline
        await _next(context);
        
        // Log the outgoing response
        Console.WriteLine($"Outgoing Response: {context.Response.StatusCode}");
    }
}