using FinalProjectAysenur.ViewModels;

namespace FinalProjectAysenur.Views
{
    public partial class FinancePage : ContentPage
    {
        private readonly FinanceViewModel _vm;
        public FinancePage(FinanceViewModel vm)
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
