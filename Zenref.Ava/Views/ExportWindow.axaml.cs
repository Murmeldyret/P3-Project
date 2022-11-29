using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class ExportWindow : Window
    {
        /*
        Button? MenuButton;
        Button? StartButton;
        Button? ExportButton;
        */
        public ExportWindow()
        {
            InitializeComponent();

            InitializeWindow();
        }
        private void InitializeComponent()
        {
#if DEBUG
            if (true)
                this.AttachDevTools();
#endif
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeWindow()
        {
        }

        /*
        private void InitializeWindow()
        {
            MenuButton = this.FindControl<Button>("menuButton");
            MenuButton.Click += (s, e) =>
            {
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.ShowDialog(this);
            };
            StartButton = this.FindControl<Button>("startButton");
            StartButton.Click += (s, e) =>
            {

            };
            ExportButton = this.FindControl<Button>("exportButton");
            StartButton.Click += (s, e) =>
            {

            };
        }
        */
    }
}
