using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Zenref.Ava.Views
{
    public partial class ExportWindow : Window
    {
        Button? MenuButton;
        Button? StartButton;
        Button? ExportButton;
        public ExportWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }
        private void InitializeWindow()
        {
            MenuButton = this.FindControl<Button>("menuButton");
            MenuButton.Click += (s, e) =>
            {
                MainWindow menuWindow = new MainWindow();
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
    }
}
