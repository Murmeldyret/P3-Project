using Avalonia.Controls;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class DatabaseView : Window
    {
        public DatabaseView()
        {
            InitializeComponent();
            DataContext = new DatabaseViewModel();
        }
    }
}
