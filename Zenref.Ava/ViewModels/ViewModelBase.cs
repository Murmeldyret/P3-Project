using ReactiveUI;
using System.ComponentModel;

namespace Zenref.Ava.ViewModels
{
    // This class serves as a base class for view model classes.
    // It implements the INotifyPropertyChanged interface and implements a method that can be used to raise the PropertyChanged event.
    public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
    {
        // PropertyChanged event that can be raised to indicate that a property value has changed.
        public event PropertyChangedEventHandler PropertyChanged;

        // This method can be called to raise the PropertyChanged event.
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}