using Avalonia.Controls;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class DatabaseView : Window
    {
        DatabaseViewModel databaseViewModel = new DatabaseViewModel();
        public DatabaseView()
        {
            InitializeComponent();
            DataContext = databaseViewModel;
        }
    }
}
