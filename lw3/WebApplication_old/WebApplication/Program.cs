using Microsoft.EntityFrameworkCore;
using WebApplication.Data;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5256",
        builder => builder.WithOrigins("http://localhost:5256")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddControllers(options =>
//{
//    // Применение конфигурации HATEOAS, если необходимо
//    options.Filters.Add<AddHateoasLinksFilter>(); // Пример использования фильтра для HATEOAS, создайте его при необходимости
//});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление Swagger (опционально, для удобства тестирования)
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseCors("AllowLocalhost5256");

app.MapControllers();



app.MapGet("/", () => "The WebApplication is working.");

app.Run();
