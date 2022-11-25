using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Zenref.Ava.ViewModels
{
    public partial class DragAndDropViewModel : ObservableObject
    {
        [ObservableProperty]
        private string filePath;

        [ObservableProperty]
        private ObservableCollection<FileInfo> files;

        public DragAndDropViewModel()
        {
            files = new ObservableCollection<FileInfo>();
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
            string[] filePaths = await openFileDialog.ShowAsync(window);
            if (filePaths != null)
            {
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    files.Add(fileInfo);
                }
            }    
        }

        private void DeleteFile(string fileName)
        {
            //files.Remove(files.Where(f => f.Name == fileName));
            Debug.WriteLine(files[0].Name);
            Debug.WriteLine(fileName);
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
