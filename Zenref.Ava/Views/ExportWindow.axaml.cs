using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Zenref.Ava.Views
{
    public partial class ExportWindow : Window
    {
        Button MenuButton;
        Button StartButton;
        Button ExportButton;
        public ExportWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }
        private void InitializeWindow()
        {
            MenuButton = this.FindControl<Button>("menuButton");
            MenuButton.Click += MenuButton_Click;
            StartButton = this.FindControl<Button>("startButton");
            ExportButton = this.FindControl<Button>("exportButton");

        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.ShowDialog(this);
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
