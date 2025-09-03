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

        // Register services
        builder.Services.AddSingleton<ConferencePlanService>();
        builder.Services.AddSingleton<NdcApp.Core.Services.INotificationService, LocalNotificationService>();
        builder.Services.AddSingleton<TalkNotificationService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}