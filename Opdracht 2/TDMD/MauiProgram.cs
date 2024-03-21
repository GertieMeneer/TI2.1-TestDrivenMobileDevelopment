using Microsoft.Extensions.Logging;
using TDMD.ApplicationLayer;
using TDMD.DomainLayer;
using TDMD.InfrastructureLayer;

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

            builder.Services.AddSingleton<IApi>(new Api());
            builder.Services.AddSingleton<ApiService>();

            builder.Services.AddTransient<LampInfoPageViewModel>();
            builder.Services.AddTransient<LampInfoPage>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
