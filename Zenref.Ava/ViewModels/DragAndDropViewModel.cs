using Avalonia.Controls;
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
            //public ObservableCollection<string> columnName = new ObservableCollection<string>()
            //{
            //Spreadsheet.ReferenceFields.Author.ToString(),
            //    Spreadsheet.ReferenceFields.Title.ToString(),
            //    Spreadsheet.ReferenceFields.PublicationType.ToString(),
            //    Spreadsheet.ReferenceFields.Publisher.ToString(),
            //    Spreadsheet.ReferenceFields.YearRef.ToString(),
            //    Spreadsheet.ReferenceFields.IdRef.ToString(),
            //    Spreadsheet.ReferenceFields.Education.ToString(),
            //    Spreadsheet.ReferenceFields.Location.ToString(),
            //    Spreadsheet.ReferenceFields.Semester.ToString(),
            //    Spreadsheet.ReferenceFields.Language.ToString(),
            //    Spreadsheet.ReferenceFields.YearReport.ToString(),
            //    Spreadsheet.ReferenceFields.OriginalRef.ToString(),
            //    Spreadsheet.ReferenceFields.Match.ToString(),
            //    Spreadsheet.ReferenceFields.Comment.ToString(),
            //    Spreadsheet.ReferenceFields.Syllabus.ToString(),
            //    Spreadsheet.ReferenceFields.Season.ToString(),
            //    Spreadsheet.ReferenceFields.ExamEvent.ToString(),
            //    Spreadsheet.ReferenceFields.Source.ToString(),
            //    Spreadsheet.ReferenceFields.Pages.ToString(),
            //    Spreadsheet.ReferenceFields.Volume.ToString(),
            //    Spreadsheet.ReferenceFields.Chapters.ToString(),
            //    Spreadsheet.ReferenceFields.BookTitle.ToString(),
            //};
            [ObservableProperty]
            public int columnPos;
            //public ObservableCollection<int> columnPos = new ObservableCollection<int>()
            //{
            //    1,
            //    2,
            //    3,
            //    4,
            //    5,
            //    6,
            //    7,
            //    8,
            //    9,
            //    10,
            //    11,
            //    12,
            //    13,
            //    14,
            //    15,
            //    16,
            //    17,
            //    18,
            //    19,
            //    20,
            //    21,
            //    22,

            //};
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
        private ObservableCollection<FileInfo> files;
        [ObservableProperty] 
        private int activeSheet = 1;
        
        public DragAndDropViewModel()
        {
            Debug.WriteLine("DragAndDropView constructor Called");
            isNextButtonEnabled = false;
            files = new ObservableCollection<FileInfo>();
            Files.CollectionChanged += Files_CollectionChanged;
            columnPositions = new ObservableCollection<ColumnPositionHandler>()
            {
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Author.ToString(),1, action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Title.ToString(),2,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.PublicationType.ToString(),3,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Publisher.ToString(),4,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.YearRef.ToString(),5,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.IdRef.ToString(),6,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Education.ToString(),7,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Location.ToString(),8,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Semester.ToString(),9,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Language.ToString(),10,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.YearReport.ToString(),11,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.OriginalRef.ToString(),12,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Match.ToString(),13,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Comment.ToString(),14,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Syllabus.ToString(),15,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Season.ToString(),16,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.ExamEvent.ToString(),17,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Source.ToString(),18,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Pages.ToString(),19,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Volume.ToString(),20,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Chapters.ToString(),21,action,this),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.BookTitle.ToString(),22,action,this),
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
            bool anyFileChosen = files.Count() != 0;
            IEnumerable<int> ints = columnPositions.Where(x => x.columnPos is > 0 and <= 22).Select(x => x.columnPos);
            int distinctColumnPosValues = ints.Distinct().Count();
            bool isColumnPosFilled = distinctColumnPosValues == ints.Count();
            Debug.WriteLine($"CanProceed evaluates to {isColumnPosFilled && anyFileChosen}");

            return isColumnPosFilled && anyFileChosen;

        }
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
            List<int> ints = columnPositions.Select(x => x.columnPos).ToList();
            CloseWindow(window);
            WeakReferenceMessenger.Default.Send<FilePathsMessage>(new FilePathsMessage(files.ToList(),ints, activeSheet));
        }
    }
}
