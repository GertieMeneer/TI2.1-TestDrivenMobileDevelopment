using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TDMD.Classes;
using TDMD.Interfaces;
using TDMD.ViewModels;

namespace TDMD
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<LampInfoPageViewModel>();
            builder.Services.AddTransient<LampInfoPage>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
