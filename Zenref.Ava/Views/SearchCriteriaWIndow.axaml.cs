using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using Zenref.Ava.Models;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class SearchCriteriaWindow : Window
    {

        public SearchCriteriaWindow()
        {
            SearchCriteriaViewModel searchCriteriaViewModel = new SearchCriteriaViewModel();
            InitializeComponent();
            DataContext = searchCriteriaViewModel;
        }

    }
}
