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
//    // ���������� ������������ HATEOAS, ���� ����������
//    options.Filters.Add<AddHateoasLinksFilter>(); // ������ ������������� ������� ��� HATEOAS, �������� ��� ��� �������������
//});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� Swagger (�����������, ��� �������� ������������)
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseCors("AllowLocalhost5256");

app.MapControllers();



app.MapGet("/", () => "The WebApplication is working.");

app.Run();
