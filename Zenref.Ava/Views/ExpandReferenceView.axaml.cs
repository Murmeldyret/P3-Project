using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Views
{
    public partial class ExpandReferenceView : Window
    {
        public ExpandReferenceView()
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
