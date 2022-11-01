using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class ExpandReferenceWindow : Window
    {
        public ExpandReferenceWindow()
        {
            InitializeComponent();
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
