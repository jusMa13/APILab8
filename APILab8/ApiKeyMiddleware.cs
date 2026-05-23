using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
       
        if (context.Request.Method == "OPTIONS")
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; 
            await context.Response.WriteAsync("Falta la API Key en la cabecera (X-Api-Key).");
            return;
        }

        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>("ApiKeySettings:ApiKey");

        if (!apiKey!.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401; 
            await context.Response.WriteAsync("API Key no valida.");
            return;
        }

        await _next(context);
    }
}