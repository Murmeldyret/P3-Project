using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Zenref.Ava.Views
{
    public partial class MainWindow : Window
    {
        Button? KnownReferencesButton;
        Button? IdentifyReferencesButton;
        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            KnownReferencesButton = this.FindControl<Button>("knownReferencesButton");
            KnownReferencesButton.Click += (s, e) =>
            {
                DatabaseWindow databaseWindow = new DatabaseWindow();
                databaseWindow.ShowDialog(this);
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
