using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Zenref.Ava.Models;

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
                Debug.WriteLine(FilePath);
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
