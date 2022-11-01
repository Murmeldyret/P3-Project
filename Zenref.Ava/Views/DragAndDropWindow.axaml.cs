using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class DragAndDropWindow : Window
    {
        Button? ImportButton;
        Button? CancelButton;
        Button? NextButton;

        public DragAndDropWindow()
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
            ImportButton = this.FindControl<Button>("importButton");
            ImportButton.Click += (s, e) =>
            {
                    
            };
            CancelButton = this.FindControl<Button>("cancelButton");
            CancelButton.Click += (s, e) => 
            {
                this.Close();
            };
            NextButton = this.FindControl<Button>("nextButton");
            NextButton.Click += (s, e) =>
            {
                ExportWindow exportWindow = new ExportWindow();
                exportWindow.ShowDialog(this);
            };
        }
    }
}
