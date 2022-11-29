using Avalonia.Controls;
using Avalonia.Input;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Markup.Xaml;
using Zenref.Ava.ViewModels;
using zenref.Core.Models;
using zenref.Core.Services;

namespace Zenref.Ava.Views
{
    public partial class DatabaseView : Window
    {
        // DatabaseViewModel databaseViewModel = new DatabaseViewModel();
        
        public DatabaseView()
        {
            // InitializeComponent();
            // DataContext = databaseViewModel;
            DataContext = App.ServiceProvider.GetService<ReferencesService>();
            AvaloniaXamlLoader.Load(this);
        }

        // private void SearchFilter(object sender, KeyEventArgs e)
        // {
        //     TextBox? textBox = sender as TextBox;
        //     if (textBox?.Text != "")
        //     {
        //         var filteredList = databaseViewModel.References.Where(x => x.Author.ToLower().Contains(textBox.Text.ToLower()));
        //         databaseViewModel.FilteredReferences = filteredList;
        //     }
        //     else
        //     {
        //         databaseViewModel.FilteredReferences = databaseViewModel.References;
        //     }
        // }
    }
}
