using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Zenref.Ava.ViewModels
{
    public partial class DragAndDropViewModel : ObservableObject
    {
        [ObservableProperty]
        private string filePath;

        public DragAndDropViewModel()
        {
        }


        [RelayCommand]
        private async void OpenFileDialog (Window window)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Directory = @"C:\",
                Title = "Browse for excel file",
                AllowMultiple = true,
            };
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Excel", Extensions = { "xlsx", "xlsm" } });
            string[] result = await openFileDialog.ShowAsync(window);
            if (result != null)
            {
                FilePath = string.Join(Environment.NewLine, result);
            }
        }
        private void CloseWindow (Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

    }
}
