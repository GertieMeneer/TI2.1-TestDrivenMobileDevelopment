using CommunityToolkit.Maui;
using Eindopdracht.Domain;
using Eindopdracht.ViewModels;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Eindopdracht.ApplicationServices;
using Eindopdracht.Infrastructure;

namespace Eindopdracht
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IDatabase>(new Database());
            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddSingleton<INsApi>(new NsApi());
            builder.Services.AddSingleton<NsApiService>();

            builder.Services.AddTransient<StationDetailViewModel>();
            builder.Services.AddTransient<StationDetailPage>();
            builder.Services.AddTransient<MainViewModel>();

            return builder.Build();
        }
    }
}
