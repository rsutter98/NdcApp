using Microsoft.Extensions.Logging;
using NdcApp.Core.Services;
using NdcApp.Services;
using Plugin.LocalNotification;

namespace NdcApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register core services with interfaces
        builder.Services.AddSingleton<ITalkRatingService, TalkRatingService>();
        builder.Services.AddSingleton<IConferencePlanService, ConferencePlanService>();
        builder.Services.AddSingleton<ITalkService, TalkService>();
        builder.Services.AddSingleton<ITalkFilterService, TalkFilterService>();
        builder.Services.AddSingleton<ILoggerService, LoggerService>();
        builder.Services.AddSingleton<IErrorHandlingService, ErrorHandlingService>();
        
        // Register platform-specific services
        builder.Services.AddSingleton<NdcApp.Core.Services.INotificationService, LocalNotificationService>();
        builder.Services.AddSingleton<TalkNotificationService>();
        builder.Services.AddSingleton<GlobalExceptionHandler>();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#else
        builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif
        
        // Add console logging for better debugging
        builder.Logging.AddConsole();

        var app = builder.Build();
        
        // Initialize global exception handler
        var globalExceptionHandler = app.Services.GetRequiredService<GlobalExceptionHandler>();
        globalExceptionHandler.RegisterGlobalHandlers();

        return app;
    }
}