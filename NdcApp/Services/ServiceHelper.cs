using Microsoft.Extensions.DependencyInjection;

namespace NdcApp
{
    /// <summary>
    /// Helper class for accessing dependency injection services across the application.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Gets a service of the specified type from the dependency injection container.
        /// </summary>
        /// <typeparam name="TService">The type of service to retrieve.</typeparam>
        /// <returns>The service instance, or null if not found.</returns>
        public static TService? GetService<TService>() => Current.GetService<TService>();

        /// <summary>
        /// Gets the current service provider for the application.
        /// </summary>
        public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
            MauiWinUIApplication.Current.Services;
#elif ANDROID
            MauiApplication.Current.Services;
#elif IOS || MACCATALYST
            MauiUIApplicationDelegate.Current.Services;
#else
            null!;
#endif
    }
}