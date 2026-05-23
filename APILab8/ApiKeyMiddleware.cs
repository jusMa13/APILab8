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
        // NUEVA CONDICIÓN: Si es una petición de control CORS (OPTIONS), se deja pasar directo
        if (context.Request.Method == "OPTIONS")
        {
            await _next(context);
            return;
        }

        // 1. Revisar si viene la cabecera con la API Key
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // No autorizado
            await context.Response.WriteAsync("Falta la API Key en la cabecera (X-Api-Key).");
            return;
        }

        // 2. Extraer la clave configurada en el appsettings.json
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>("ApiKeySettings:ApiKey");

        // 3. Comparar si son iguales
        if (!apiKey!.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401; // No autorizado
            await context.Response.WriteAsync("API Key no valida.");
            return;
        }

        // Si todo está bien, continuar el camino hacia el controlador
        await _next(context);
    }
}