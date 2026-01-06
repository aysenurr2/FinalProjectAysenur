using FinalProjectAysenur.Views;

namespace FinalProjectAysenur
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddPetPage), typeof(AddPetPage));
        }
    }
}
