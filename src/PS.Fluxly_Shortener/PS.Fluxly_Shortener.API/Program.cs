using PS.Fluxly_Shortener.API.Configuration;
using PS.Fluxly_Shortener.API.Services.Interfaces;
using PS.Fluxly_Shortener.API.Services;
using PS.Fluxly_Shortener.API.Storage.Interfaces;
using PS.Fluxly_Shortener.API.Storage;





var builder = WebApplication.CreateBuilder(args);


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
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}