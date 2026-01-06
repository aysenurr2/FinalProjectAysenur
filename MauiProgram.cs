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
            builder.Services.AddSingleton<DatabaseService>();

            // ViewModels
            builder.Services.AddTransient<PetViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<OwnerViewModel>();
            builder.Services.AddTransient<AppointmentViewModel>();
            builder.Services.AddTransient<TreatmentViewModel>();
            builder.Services.AddTransient<FinanceViewModel>();

            // Sayfaların Kaydı
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddPetPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<OwnerPage>();
            builder.Services.AddTransient<AppointmentPage>();
            builder.Services.AddTransient<TreatmentPage>();
            builder.Services.AddTransient<FinancePage>();

            return builder.Build();
        }
    }
}
