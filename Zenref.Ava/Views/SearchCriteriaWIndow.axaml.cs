using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class SearchCriteriaWindow : Window
    {
        Button AddButton;
        Button CancelButton;
        public SearchCriteriaWindow()
        {
            InitializeComponent();

            InitializeWindow();
        }

        private void InitializeWindow()
        {
            AddButton = this.FindControl<Button>("addButton");
            AddButton.Click += AddButton_Click;
            CancelButton = this.FindControl<Button>("cancelButton");
            CancelButton.Click += CancelButton_Click;


        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
