using Avalonia.Controls;
using Avalonia.Input;
using Zenref.Ava.ViewModels;
using System.IO;
using Avalonia.Interactivity;

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
