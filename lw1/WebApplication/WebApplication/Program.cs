using ClassLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();


var resultManager = new ResultManager();
var stackManager = new StackManager();

app.MapGet("{qm}.PAA", context =>
{
    var response = $"RESULT {resultManager.GetResult()}";

    context.Response.ContentType = "text/plain";
    return context.Response.WriteAsync(response);
});

app.MapPost("{qm}.PAA", async context =>
{
    var result = context.Request.Query["RESULT"];

    resultManager.SetResult(int.Parse(result));

    var response = $"RESULT = {resultManager.GetResult()}";

    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync(response);
});


app.MapPut("{qm}.PAA", context =>
{
    var addParameter = context.Request.Query["ADD"];

    stackManager.Push(int.Parse(addParameter));
    resultManager.AddToResult(int.Parse(addParameter));

    var response = $"RESULT = {resultManager.GetResult()}";

    context.Response.ContentType = "text/plain";
    return context.Response.WriteAsync(response);
});

app.MapDelete("{qm}.PAA", context =>
{
    if (stackManager.Pop(out int poppedValue))
    {
        resultManager.SubtractFromResult(poppedValue);
        var response = $"RESULT = {resultManager.GetResult()}";

        context.Response.ContentType = "text/plain";
        return context.Response.WriteAsync(response);
    }

    var errorResponse = "Stack is empty, nothing to pop.";
    context.Response.StatusCode = 400;
    context.Response.ContentType = "text/plain";
    return context.Response.WriteAsync(errorResponse);
});



// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
