using FinalProjectAysenur.ViewModels;

namespace FinalProjectAysenur.Views
{
    public partial class AppointmentPage : ContentPage
    {
        private readonly AppointmentViewModel _vm;
        public AppointmentPage(AppointmentViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(_vm.LoadAppointmentsCommand.CanExecute(null))
                _vm.LoadAppointmentsCommand.Execute(null);
        }
    }
}