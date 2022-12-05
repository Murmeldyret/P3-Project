using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {

        }

        private void OpenDatabaseView(Window window)
        {
            DatabaseView databaseView = new DatabaseView();
            databaseView.ShowDialog(window);
        }
        private void OpenExportView(Window window)
        {
            ExportView exportView = new ExportView();
            exportView.ShowDialog(window);
        }
    }
}