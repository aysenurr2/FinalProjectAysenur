using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinalProjectAysenur.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string name = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(name);
            return true;
        }
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
