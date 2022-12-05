using Avalonia.Controls;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class SearchCriteriaView : Window
    {
        public SearchCriteriaView()
        {
            SearchCriteriaViewModel searchCriteriaViewModel = new SearchCriteriaViewModel();
            InitializeComponent();
            DataContext = searchCriteriaViewModel;
        }
    }
}
