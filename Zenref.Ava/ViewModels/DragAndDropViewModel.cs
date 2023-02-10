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
        // List of FileInfo objects representing the paths of the Excel files.
        public List<FileInfo> FilePaths { get; }
        
        // List of integers representing the positions of the columns in the Excel files.
        public  List<int> ColumnPositions { get; }
        
        // Integer representing the active sheet in the Excel files.
        public  int ActiveSheet { get; } 
        
        /// <summary>
        /// Constructor for creating a new FilePathsMessage object.
        /// </summary>
        /// <param name="filePaths">List of FileInfo objects representing the paths of the Excel files.</param>
        /// <param name="columnPositions">List of integers representing the positions of the columns in the Excel files.</param>
        /// <param name="activeSheet">Integer representing the active sheet in the Excel files.</param>
        public FilePathsMessage (List<FileInfo> filePaths, List<int> columnPositions, int activeSheet)
        {
            FilePaths = filePaths;
            ColumnPositions = columnPositions;
            ActiveSheet = activeSheet;
        }
    }

    public partial class DragAndDropViewModel : ObservableObject
    {
        /// <summary>
        /// Class that handles the column positions in the DragAndDropViewModel
        /// </summary>
        [ObservableObject]
        public partial class ColumnPositionHandler
        {
            /// <summary>
            /// Constructor for creating a new ColumnPositionHandler object
            /// </summary>
            /// <param name="name">The name of the column</param>
            /// <param name="value">The position of the column</param>
            /// <param name="action">The action to be performed when the column position changes</param>
            /// <param name="viewModel">The DragAndDropViewModel that the ColumnPositionHandler belongs to</param>
            public ColumnPositionHandler(string name, int value, Action<DragAndDropViewModel> action, DragAndDropViewModel viewModel)
            {
                // Assign the passed in name to the columnName field
                columnName = name;
                // Assign the passed in value to the columnPos field
                columnPos = value;
                // Register the ColumnPositionHandler_PropertyChanged event handler
                this.PropertyChanged += ColumnPositionHandler_PropertyChanged;
                // Assign the passed in action to the Action field
                Action = action;
                // Assign the passed in viewModel to the ViewModel field
                ViewModel = viewModel;
            }
            
            /// <summary>
            /// Event handler that is called when the column position changes. It invokes the action passed in the constructor
            /// </summary>
            /// <param name="sender">The object that raised the event</param>
            /// <param name="e">Event arguments</param>
            private void ColumnPositionHandler_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                // Invokes the action passed in the constructor
                Action.Invoke(ViewModel);
            }
            
            // Holds the action to be performed when the column position changes
            private Action<DragAndDropViewModel> Action;
            // Holds the DragAndDropViewModel that the ColumnPositionHandler belongs to
            private DragAndDropViewModel ViewModel;
            

            // The name of the column
            [ObservableProperty]
            public string columnName;

            // The position of the column
            [ObservableProperty]
            public int columnPos;
        }

        private Action<DragAndDropViewModel> action = (x) =>
        {
            // Writes a debug message to the console
            Debug.WriteLine("Column Position Changed");
            
            // Enables the next button if x.CanProceed() returns true
            x.IsNextButtonEnabled = x.CanProceed();
        };
        
        // Holds a collection of ColumnPositionHandler
        [ObservableProperty]
        private ObservableCollection<ColumnPositionHandler> columnPositions;
        
        // Determines whether the next button is enabled
        [ObservableProperty]
        private bool isNextButtonEnabled;
        
        // Holds a list of file paths
        [ObservableProperty]
        private List<string> filePath;
        
        // Holds a collection of FileInfo objects
        [ObservableProperty]
        private ObservableCollection<FileInfo>? files;
        
        // Holds the active sheet number
        [ObservableProperty] 
        private int activeSheet = 1;
        
        // : Initializes a new instance of the DragAndDropViewModel class
        public DragAndDropViewModel()
        {
            // : Writes a debug message to the console
            Debug.WriteLine("DragAndDropView constructor Called");
            
            // : Initially disables the next button
            isNextButtonEnabled = false;
            
            // : Creates a new ObservableCollection of FileInfo objects
            files = new ObservableCollection<FileInfo>();
            
            // : Registers the Files_CollectionChanged event handler
            Files.CollectionChanged += Files_CollectionChanged;
            
            // : Creates a new ObservableCollection of ColumnPositionHandler objects
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
            // : Writes a debug message to the console
            Debug.WriteLine("File was chosen");
            
            // : Enables or disables the next button depending on whether or not the user can proceed
            IsNextButtonEnabled = CanProceed();
        }

        /// <summary>
        /// Determines whether or not the user can press the next button by checking that any file is chosen, and that the column position is correctly filled
        /// </summary>
        /// <returns>True if the user can proceed, false otherwise</returns>
        private bool CanProceed()
        {
            // : Determines if any files were chosen
            bool anyFileChosen;
            if (files is null)
            {
                anyFileChosen = false;
            }
            else
            {
                anyFileChosen = files.Count() != 0;
            }
            
            // : Gets the column positions that are greater than 0 and selects their values
            IEnumerable<int> ints = columnPositions
                .Where(x => x.columnPos is > 0)
                .Select(x => x.columnPos);
            
            // : Counts the number of distinct column positions
            int distinctColumnPosValues = ints
                .Distinct()
                .Count();
            
            // : Determines if the column positions are filled
            bool isColumnPosFilled = distinctColumnPosValues == ints.Count();
            
            // : Writes a debug message to the console
            Debug.WriteLine($"CanProceed evaluates to {isColumnPosFilled && anyFileChosen}");
            
            // : Returns true if the column positions are filled and a file was chosen, otherwise false
            return isColumnPosFilled && anyFileChosen;

        }
        /// <summary>
        /// Opens a file dialog to select excel files and adds these to a collection.
        /// </summary>
        [RelayCommand]
        private async void OpenFileDialog (Window window)
        {
            // : Creates a new OpenFileDialog with title, "Browse for excel file" and allows multiple selections.
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Browse for excel file",
                AllowMultiple = true,
            };
            
            // : Adds filter to only show excel files
            openFileDialog.Filters!.Add(new FileDialogFilter() { Name = "Excel", Extensions = { "xlsx", "xlsm" } });
            
            // : Opens the file dialog and wait for user to select files
            string[] filePaths = await openFileDialog.ShowAsync(window);
            
            // : If files are selected, then add them to the files collection
            if (filePaths != null)
            {
                foreach (string filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    files.Add(fileInfo);
                } 
                // : copy the selected files' path to filePath variable
                filePath = filePaths.ToList();
            }
        }
        /// <summary>
        /// Event fired when object is dragged over element. It limits what types of objects can be dropped in the drop element.
        /// </summary>
        public void DragOver(object sender, DragEventArgs e)
        {
            // : Cast the data as a list of file names
            List<string> fileList = (List<string>)e.Data.GetFileNames();
            e.DragEffects = e.DragEffects & DragDropEffects.Copy;
            
            // if(!fileList.All(f => Path.GetExtension(f) == ".xlsx")) // Better way??. Self-note
            // : Check if the data is file names and check if all files are xlsx files
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
            // : Check if the dropped object contains file names
            if (e.Data.Contains(DataFormats.FileNames))
            {
                // : Get the file paths of the dropped files
                List<string> filePaths = (List<string>)e.Data.GetFileNames();
                
                // : Iterate through each file path
                foreach (string filePath in filePaths)
                {
                    // : Create a FileInfo object for the current file path
                    FileInfo fileInfo = new FileInfo(filePath);
                    
                    // : Add the FileInfo object to the files collection
                    files.Add(fileInfo);
                }
            }
        }

        private void CloseWindow (Window window)
        {
            // : Check if the window is not null
            if (window != null)
            {
                // : close the window
                window.Close();
            }
        }
        /// <summary>
        /// Confirms the files chosen by filedialog and sends a message containing the filenames, what each column contains, and what sheet to read from.
        /// </summary>
        /// <param name="window">The window that contains the filenames and properties related to it.</param>
        private void ConfirmFileChoices(Window window)
        {
            // : Select the column positions from the columnPositions object and store it in a list
            List<int> ints = columnPositions.Select(x => x.columnPos).ToList();
            
            // : Close the window
            CloseWindow(window);
            
            // : Send a message containing the filenames, column positions and active sheet
            WeakReferenceMessenger.Default.Send<FilePathsMessage>(new FilePathsMessage(files.ToList(),ints, activeSheet));
        }
    }
}
