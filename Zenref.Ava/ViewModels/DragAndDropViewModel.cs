using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <summary>
        /// Opens a file dialog to select excel files and adds these to a collection.
        /// </summary>
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
        /// <summary>
        /// Event fired when object is dragged over element. It limits what types of objects can be dropped in the drop element.
        /// </summary>
        public void DragOver(object sender, DragEventArgs e)
        {
            List<string> fileList = (List<string>)e.Data.GetFileNames();
            e.DragEffects = e.DragEffects & DragDropEffects.Copy;
            if (!e.Data.Contains(DataFormats.FileNames) || !fileList.All(f => f.Contains("xlsx")))
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        /// <summary>
        /// Event fired when object is dropped in drop element. It determines if the dropped object is a file and adds the dropped file to a collection.
        /// </summary>
        public void FileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                List<string> filePaths = (List<string>)e.Data.GetFileNames();
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    files.Add(fileInfo);
                }
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
