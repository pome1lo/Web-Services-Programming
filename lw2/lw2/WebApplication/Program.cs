var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost7248",
        builder => builder.WithOrigins("https://localhost:7248")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Add services to the container (in this case, no specific services are needed)
var app = builder.Build();

// Global state for the stack and RESULT
var globalStack = new Stack<int>();
int result = 0;

// Define the endpoint mapping for HTTP methods
app.MapGet("{qm}.PAA", async context =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync($"{{\"RESULT\": {result}}}");
});

app.MapPost("{qm}.PAA", async context =>
{
    try
    {
        var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);

        if (data != null && data.ContainsKey("RESULT"))
        {
            result = data["RESULT"];
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync($"{{\"RESULT\": {result}}}");
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid or missing RESULT parameter.");
        }
    }
    catch
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid request body.");
    }
});

app.MapPut("{qm}.PAA", async context =>
{
    try
    {
        var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);

        if (data != null && data.ContainsKey("ADD"))
        {
            int add = data["ADD"];
            globalStack.Push(add);
            result += add;  // Update RESULT with the new value added to the stack
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync($"{{\"RESULT\": {result}}}");
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid or missing ADD parameter.");
        }
    }
    catch
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid request body.");
    }
});

app.MapDelete("{qm}.PAA", async context =>
{
    if (globalStack.Count > 0)
    {
        int poppedValue = globalStack.Pop();
        result -= poppedValue;  // Update RESULT after popping the top element
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"RESULT\": {result}}}");
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Stack is empty, nothing to pop.");
    }
});

// Configure the HTTP request pipeline
app.UseCors("AllowLocalhost7248");
app.UseHttpsRedirection();
app.UseRouting();

app.Run();
