public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _configuredApiKey;
    private const string APIKEY_HEADER = "X-API-KEY";


    public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _configuredApiKey = config["ApiKey"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;

        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        if (!_configuredApiKey.Equals(extractedApiKey.ToString()))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}
