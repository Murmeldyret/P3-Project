using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;

namespace Zenref.Ava.ViewModels
{
    /// <summary>
    /// Message class sending and receiving strings containing paths for Excel files.
    /// </summary>
    public class FilePathsMessage
    {
        public List<FileInfo> FilePaths { get; init; }

        public FilePathsMessage (List<FileInfo> filePaths)
        {
            FilePaths = filePaths;
        }
    }
    public partial class DragAndDropViewModel : ObservableObject
    {
        WeakReferenceMessenger messenger = new WeakReferenceMessenger();


        [ObservableProperty]
        private List<string> filePath;

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
            filePath = filePaths.ToList();
        }

        private void CloseWindow (Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        private void ConfirmFileChoices(Window window)
        {
            WeakReferenceMessenger.Default.Send<FilePathsMessage>(new FilePathsMessage(files.ToList()));
            CloseWindow(window);
        }
    }
}
