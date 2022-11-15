using Avalonia.Controls;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class ExportView : Window
    {
        public ExportView()
        {
            InitializeComponent();
            DataContext = new ExportViewModel();
        }
    }
}
