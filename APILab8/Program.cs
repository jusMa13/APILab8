var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Origen de tu app Angular
              .WithHeaders("X-Api-Key")             // Permite explícitamente tu cabecera
              .WithMethods("GET", "POST", "OPTIONS"); // Permite los métodos necesarios
    });
});

var app = builder.Build();

app.UseCors("PermitirAngular");

// Llama al archivo externo ApiKeyMiddleware de forma limpia
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();