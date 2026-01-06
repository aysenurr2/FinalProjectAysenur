using FinalProjectAysenur.ViewModels;

namespace FinalProjectAysenur.Views
{
    public partial class TreatmentPage : ContentPage
    {
        private readonly TreatmentViewModel _vm;
        public TreatmentPage(TreatmentViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(_vm.LoadDataCommand.CanExecute(null))
                _vm.LoadDataCommand.Execute(null);
        }
    }
}
