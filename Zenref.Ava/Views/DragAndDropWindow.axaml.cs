using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class DragAndDropWindow : Window
    {
        Button ImportButton;
        Button CancelButton;
        Button NextButton;

        public DragAndDropWindow()
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
            ImportButton = this.FindControl<Button>("importButton");
            ImportButton.Click += ImportButton_Click;
            CancelButton = this.FindControl<Button>("cancelButton");
            CancelButton.Click += CancelButton_Click;
            NextButton = this.FindControl<Button>("nextButton");
            NextButton.Click += NextButton_Click;
        }
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void NextButton_Click(object sender, RoutedEventArgs e) 
        {
            ExportWindow exportWindow = new ExportWindow();
            exportWindow.ShowDialog(this);
        }
    }
}
