using NdcApp.Core.Services;
using NdcApp.Preview.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register core services (using interfaces from NdcApp.Core)
builder.Services.AddSingleton<ITalkRatingService, TalkRatingService>();
builder.Services.AddSingleton<IConferencePlanService, ConferencePlanService>();
builder.Services.AddSingleton<ITalkService, TalkService>();
builder.Services.AddSingleton<ITalkFilterService, TalkFilterService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IErrorHandlingService, ErrorHandlingService>();

// Register preview-specific services
builder.Services.AddSingleton<INotificationService, PreviewNotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add custom route for mobile setup status page
app.MapGet("/status", async context =>
{
    var statusFilePath = Path.Combine(app.Environment.WebRootPath, "status.html");
    if (File.Exists(statusFilePath))
    {
        var content = await File.ReadAllTextAsync(statusFilePath);
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(content);
    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Status page not found");
    }
});

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();