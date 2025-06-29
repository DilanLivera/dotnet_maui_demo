using Microsoft.Extensions.Logging;
using PinCodeAuth.Features.PinCodeAuthentication.Application;
using PinCodeAuth.Features.PinCodeAuthentication.Presentation;
using CommunityToolkit.Maui;

namespace PinCodeAuth;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<AuthenticatePinHandler>();
        builder.Services.AddTransient<PinCodeViewModel>();
        builder.Services.AddTransient<PinCodeView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}