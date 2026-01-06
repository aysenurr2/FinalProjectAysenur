namespace FinalProjectAysenur
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = serviceProvider.GetService<Views.LoginPage>();
        }
    }
}
