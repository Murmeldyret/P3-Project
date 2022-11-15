using Avalonia.Controls;
using Avalonia.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Zenref.Ava.ViewModels
{
    public class DragAndDropViewModel : ViewModelBase
    {

        public DragAndDropViewModel()
        {
            string filePath = "asd";
            //this.OpenFileDialogCommand = ReactiveCommand.Create<Window>(FileDialog);
            //this.CloseWindowCommand = ReactiveCommand.Create<Window>(CloseWindow);
        }


        //public ReactiveCommand<Window, Unit> OpenFileDialogCommand { get; }
        //public ReactiveCommand<Window, Unit> CloseWindowCommand { get; }

        private async void OpenFileDialog (Window window)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse for excel file",
                AllowMultiple = true,
            };
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Excel", Extensions = { "xlsx", "xlsm" } });
            string[] result = await openFileDialog.ShowAsync(window);
            if (result != null)
            {
                string filePath = string.Join(Environment.NewLine, result);
                Debug.WriteLine(filePath);
            }
        }
        private void CloseWindow (Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void DragOver(object sender, DragEventArgs e)
        {
            e.DragEffects = e.DragEffects & DragDropEffects.Copy;
            if (!e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        private void FileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                string filePath = string.Join(Environment.NewLine, e.Data.GetFileNames());
                Debug.WriteLine(filePath);
            }
        }
    }
}
