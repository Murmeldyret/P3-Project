using System;
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
            Border border = this.FindControl<Border>("CopyTarget")
            border
            ImportButton = this.FindControl<Button>("importButton");
            ImportButton.Click += async (s, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Browse for excel file",
                };
                openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Excel", Extensions = { "xlsx" } });
                string[] result = await openFileDialog.ShowAsync(this);
                if (result != null)
                {
                    string filePath = string.Join(" ", result);
                    TextBlock textBlock = this.FindControl<TextBlock>("fileNameTextBlock");
                    textBlock.Text = filePath;
                }
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
        private void DragDropEvent(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
