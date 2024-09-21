var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost7248",
        builder => builder.WithOrigins("https://localhost:7248")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

var globalStack = new Stack<int>();
int result = 0;

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
            result += add;
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
        result -= poppedValue;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"RESULT\": {result}}}");
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Stack is empty, nothing to pop.");
    }
});


app.UseCors("AllowLocalhost7248");
app.UseHttpsRedirection();
app.UseRouting();

app.Run();
