using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using Zenref.Ava.Models.Spreadsheet;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Zenref.Ava.ViewModels
{
    /// <summary>
    /// Message class sending and receiving strings containing paths for Excel files.
    /// </summary>
    public class FilePathsMessage
    {
        public List<FileInfo> FilePaths { get; }
        public  List<int> ColumnPositions { get; }
        public  int ActiveSheet { get; } 
        
        public FilePathsMessage (List<FileInfo> filePaths, List<int> columnPositions, int activeSheet)
        {
            FilePaths = filePaths;
            ColumnPositions = columnPositions;
            ActiveSheet = activeSheet;
        }
    }

    public partial class DragAndDropViewModel : ObservableObject
    {
        [ObservableObject]
        public partial class ColumnPositionHandler
        {
            public ColumnPositionHandler(string name, int value, Action<DragAndDropViewModel> action, DragAndDropViewModel viewModel)
            {
                columnName = name;
                columnPos = value;
                this.PropertyChanged += ColumnPositionHandler_PropertyChanged;
                Action = action;
                ViewModel = viewModel;
            }

            private void ColumnPositionHandler_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                Action.Invoke(ViewModel);
            }

            private Action<DragAndDropViewModel> Action;
            private DragAndDropViewModel ViewModel;
            [ObservableProperty]
            public string columnName;

            [ObservableProperty]
            public int columnPos;
        }

        private Action<DragAndDropViewModel> action = (x) =>
        {
            Debug.WriteLine("Column Position Changed");
            x.IsNextButtonEnabled = x.CanProceed();
        };
        [ObservableProperty]
        private ObservableCollection<ColumnPositionHandler> columnPositions;

        [ObservableProperty]
        private bool isNextButtonEnabled;

        [ObservableProperty]
        private List<string> filePath;

        [ObservableProperty]
        private ObservableCollection<FileInfo>? files;
        [ObservableProperty] 
        private int activeSheet = 1;
        
        public DragAndDropViewModel()
        {
            Debug.WriteLine("DragAndDropView constructor Called");
            isNextButtonEnabled = false;
            files = new ObservableCollection<FileInfo>();
            Files.CollectionChanged += Files_CollectionChanged;
            int defaultColumnPositions = 1;
            columnPositions = new ObservableCollection<ColumnPositionHandler>()
            {
                new ColumnPositionHandler("Forfatter",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Titel",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Publikationstype",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Forlag",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("År (Reference)",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Reference Id",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Uddannelse",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Uddannelsessted",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Semester",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Sprog",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("År (Rapport)",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Henvisning",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Match",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Kommentar",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Pensum",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Sæson".ToString(),defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Eksamensbegivenhed",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Kilde",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Sidetal",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Bind",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Kapitler",defaultColumnPositions++,action,this),
                new ColumnPositionHandler("Bogtitel",defaultColumnPositions++,action,this),
            };
        }

        private void Files_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("File was chosen");
            IsNextButtonEnabled = CanProceed();
        }

        /// <summary>
        /// Determines whether or not the user can press the next button by checking that any file is chosen, and that the column position is correctly filled
        /// </summary>
        /// <returns>True if the user can proceed, false otherwise</returns>
        private bool CanProceed()
        {
            bool anyFileChosen;
            if (files is null)
            {
                anyFileChosen = false;
            }
            else
            {
                anyFileChosen = files.Count() != 0;
            }

            IEnumerable<int> ints = columnPositions
                .Where(x => x.columnPos is > 0)
                .Select(x => x.columnPos);

            int distinctColumnPosValues = ints
                .Distinct()
                .Count();
            bool isColumnPosFilled = distinctColumnPosValues == ints.Count();
            Debug.WriteLine($"CanProceed evaluates to {isColumnPosFilled && anyFileChosen}");

            return isColumnPosFilled && anyFileChosen;

        }
        /// <summary>
        /// Opens a file dialog to select excel files and adds these to a collection.
        /// </summary>
        [RelayCommand]
        private async void OpenFileDialog (Window window)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Browse for excel file",
                AllowMultiple = true,
            };
            openFileDialog.Filters!.Add(new FileDialogFilter() { Name = "Excel", Extensions = { "xlsx", "xlsm" } });
            string[] filePaths = await openFileDialog.ShowAsync(window);
            if (filePaths != null)
            {
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    files.Add(fileInfo);
                }
            filePath = filePaths.ToList();
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
        /// <summary>
        /// Confirms the files chosen by filedialog and sends a message containing the filenames, what each column contains, and what sheet to read from.
        /// </summary>
        /// <param name="window">The window that contains the filenames and properties related to it.</param>
        private void ConfirmFileChoices(Window window)
        {
            List<int> ints = columnPositions.Select(x => x.columnPos).ToList();
            CloseWindow(window);
            WeakReferenceMessenger.Default.Send<FilePathsMessage>(new FilePathsMessage(files.ToList(),ints, activeSheet));
        }
    }
}
