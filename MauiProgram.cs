using FinalProjectAysenur.Services;
using FinalProjectAysenur.ViewModels;
using Microsoft.Extensions.Logging;
using FinalProjectAysenur.Views;
namespace FinalProjectAysenur
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
        
            // Servislerin Kaydı
            builder.Services.AddSingleton<Services.DatabaseService>();
            builder.Services.AddSingleton<ViewModels.PetViewModel>();
            builder.Services.AddTransient<Views.MainPage>();


            // Sayfaların Kaydı
            builder.Services.AddTransient<Views.MainPage>();
            builder.Services.AddTransient<Views.AddPetPage>();
            return builder.Build();
        }
    }
}
