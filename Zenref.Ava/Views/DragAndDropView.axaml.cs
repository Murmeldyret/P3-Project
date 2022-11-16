using Avalonia.Controls;
using Avalonia.Input;
using System.Diagnostics;
using System;
using Zenref.Ava.ViewModels;

namespace Zenref.Ava.Views
{
    public partial class DragAndDropView : Window
    {
        DragAndDropViewModel dragAndDropViewModel = new DragAndDropViewModel();
        public DragAndDropView()
        {
            InitializeComponent();
            DataContext = dragAndDropViewModel;
            AddHandler(DragDrop.DropEvent, FileDrop);
            AddHandler(DragDrop.DragOverEvent, DragOver);
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
                dragAndDropViewModel.FilePath = filePath;
                Debug.WriteLine(filePath);
            }
        }
    }
}
