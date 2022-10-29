using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class SearchCriteriaWindow : Window
    {
        public SearchCriteriaWindow()
        {
            this.InitializeComponent();

            InitializeWindow();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeWindow()
        {

        }
    }
}
