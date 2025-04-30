using PS.Fluxly_Shortener.API.Configuration;
using PS.Fluxly_Shortener.API.Services.Interfaces;
using PS.Fluxly_Shortener.API.Services;
using PS.Fluxly_Shortener.API.Storage.Interfaces;
using PS.Fluxly_Shortener.API.Storage;





var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:7202")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Настройка конфигурации
builder.Services.Configure<ShortenerSettings>(builder.Configuration.GetSection("Shortener"));

// Регистрация Redis хранилища
string redisConnection = builder.Configuration.GetSection("Redis")["ConnectionString"]!;
builder.Services.AddSingleton<ILinkStorage>(new RedisLinkStorage(redisConnection));

// Регистрация сервиса сокращения ссылок
builder.Services.AddSingleton<ILinkShortenerService, LinkShortenerService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Fluxly Shortener API v1");
            options.RoutePrefix = string.Empty; // Делает Swagger доступным по корневому пути
        });
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}