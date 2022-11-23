using Avalonia.Controls;
using Avalonia.Input;
using Zenref.Ava.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

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
            List<string> fileList = (List<string>)e.Data.GetFileNames();
            e.DragEffects = e.DragEffects & DragDropEffects.Copy;
            if (!e.Data.Contains(DataFormats.FileNames) || !fileList.All(c => c.Contains("xlsx")))
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        private void FileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                List<string> filePaths = (List<string>)e.Data.GetFileNames();
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    dragAndDropViewModel.Files.Add(fileInfo);
                }
            }
        }
    }
}
