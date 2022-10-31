using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Zenref.Ava.Views
{
    public partial class MenuWindow : Window
    {
        Button KnownReferencesButton;
        Button IdentifyReferencesButton;
        public MenuWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            KnownReferencesButton = this.FindControl<Button>("knownReferencesButton");
            KnownReferencesButton.Click += KnownReferencesButton_Click;
            IdentifyReferencesButton = this.FindControl<Button>("identifyReferencesButton");
            IdentifyReferencesButton.Click += IdentifyReferencesButton_Click;
        }
        private void KnownReferencesButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog(this);
        }
        private void IdentifyReferencesButton_Click(object sender, RoutedEventArgs e)
        {
            DragAndDropWindow dragAndDropWindow = new DragAndDropWindow();
            dragAndDropWindow.ShowDialog(this);
        }
    }
}
