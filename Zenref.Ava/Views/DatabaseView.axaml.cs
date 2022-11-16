using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Zenref.Ava.Models;
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

        private void SearchFilter(object sender, KeyEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox?.Text != "")
            {
                var filteredList = databaseViewModel.References.Where(x => x.Author.ToLower().Contains(textBox.Text.ToLower()));
                databaseViewModel.FilteredReferences = filteredList;
            }
            else
            {
                databaseViewModel.FilteredReferences = databaseViewModel.References;
            }
        }
    }
}
