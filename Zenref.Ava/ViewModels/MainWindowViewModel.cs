using System.Windows.Input;
using Zenref.Ava.Commands;

namespace Zenref.Ava.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; }

        public MainWindowViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
