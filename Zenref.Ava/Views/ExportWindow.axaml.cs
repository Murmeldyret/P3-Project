using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Zenref.Ava.Views
{
    public partial class ExportWindow : Window
    {
        Button? ImportButton;
        Button? StartButton;
        Button? ExportButton;
        public ExportWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            ImportButton = this.FindControl<Button>("importButton");
            ImportButton.Click += (s, e) =>
            {
                DragAndDropWindow dragAndDropWindow = new DragAndDropWindow();
                dragAndDropWindow.ShowDialog(this);
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
