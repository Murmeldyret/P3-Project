using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Zenref.Ava.Views
{
    public partial class MenuWindow : Window
    {
        Button? KnownReferencesButton;
        Button? IdentifyReferencesButton;
        public MenuWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            KnownReferencesButton = this.FindControl<Button>("knownReferencesButton");
            KnownReferencesButton.Click += (s, e) =>
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog(this);
            };
            IdentifyReferencesButton = this.FindControl<Button>("identifyReferencesButton");
            IdentifyReferencesButton.Click += (s, e) =>
            {
                DragAndDropWindow dragAndDropWindow = new DragAndDropWindow();
                dragAndDropWindow.ShowDialog(this);
            };
        }
    }
}
