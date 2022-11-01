using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Zenref.Ava.Models;

namespace Zenref.Ava.Views
{
    public partial class SearchCriteriaWindow : Window
    {
        Button? AddButton;
        Button? CancelButton;
        public SearchCriteriaWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            AddButton = this.FindControl<Button>("addButton");
            AddButton.Click += (s, e) =>
            {

            };
            CancelButton = this.FindControl<Button>("cancelButton");
            CancelButton.Click += (s, e) =>
            {
                this.Close();
            };


        }
    }
}
