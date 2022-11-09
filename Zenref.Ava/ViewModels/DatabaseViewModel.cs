using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    internal class DatabaseViewModel : BaseViewModel
    {
        private void OpenImportView()
        {
            Window window = new Window
            {
                Title = "Import Window",
                Content = new ImportView()
            };
            window.Show();
        }
    }
}
