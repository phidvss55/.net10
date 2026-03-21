namespace webapi.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string relm;
    
    public AuthenticationMiddleware(RequestDelegate next, string relm)
    {
        this._next = next;
        this.relm = relm;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
        
        var authHeader = context.Request.Headers["Authorization"].ToString();
        var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
        var decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
        string[] credentials = decodedCredentials.Split(':');
        var uid = credentials[0];
        var password = credentials[1];
        if (uid != "admin" || password != "password" || relm != "admin")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
        
        await _next(context);
    }
}