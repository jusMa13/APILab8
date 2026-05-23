var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .WithHeaders("X-Api-Key")            
              .WithMethods("GET", "POST", "OPTIONS"); 
    });
});

var app = builder.Build();

app.UseCors("PermitirAngular");


app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();