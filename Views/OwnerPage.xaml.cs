using FinalProjectAysenur.ViewModels;

namespace FinalProjectAysenur.Views
{
    public partial class OwnerPage : ContentPage
    {
        private readonly OwnerViewModel _vm;
        public OwnerPage(OwnerViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
             base.OnAppearing();
             if(_vm.LoadOwnersCommand.CanExecute(null))
                 _vm.LoadOwnersCommand.Execute(null);
        }
    }
}