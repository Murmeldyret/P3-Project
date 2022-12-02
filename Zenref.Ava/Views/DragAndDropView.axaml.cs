using Avalonia.Controls;
using Avalonia.Input;
using Zenref.Ava.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Avalonia.Interactivity;
using System;

namespace Zenref.Ava.Views
{
    public partial class DragAndDropView : Window
    {
        DragAndDropViewModel dragAndDropViewModel = new DragAndDropViewModel();
        public DragAndDropView()
        {
            InitializeComponent();
            DataContext = dragAndDropViewModel;
            AddHandler(DragDrop.DropEvent, dragAndDropViewModel.FileDrop);
            AddHandler(DragDrop.DragOverEvent, dragAndDropViewModel.DragOver);
        }

        /// <summary>
        /// Method called from a button click. The method removes the associated file from the files collection.
        /// </summary>
        private void RemoveFile(object sender, RoutedEventArgs e)
        {
            Button removeFileButton = sender as Button;
            if (removeFileButton.DataContext is FileInfo)
            {
                FileInfo file = (FileInfo)removeFileButton.DataContext;
                dragAndDropViewModel.Files.Remove(file);
            }
        }
    }
}
