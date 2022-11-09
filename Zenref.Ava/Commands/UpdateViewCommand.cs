using Zenref.Ava.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zenref.Ava.Views;

namespace Zenref.Ava.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public UpdateViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Database")
            {
                viewModel.SelectedViewModel = new DatabaseViewModel();
            }
            else if (parameter.ToString() == "Export")
            {
                viewModel.SelectedViewModel = new ExportViewModel();
            }
        }
    }
}
