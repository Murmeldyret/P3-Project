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
        public List<FileInfo> FilePaths { get; init; }

        public FilePathsMessage (List<FileInfo> filePaths)
        {
            FilePaths = filePaths;
        }
    }

    public partial class DragAndDropViewModel : ObservableObject
    {
        [ObservableObject]
       public partial class ColumnPositionHandler
        {
            public ColumnPositionHandler(string name, int value)
            {
                columnName = name;
                columnPos = value;
            }
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
        [ObservableProperty]
        private ObservableCollection<ColumnPositionHandler> columnPositions = new ObservableCollection<ColumnPositionHandler>()
        {
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Author.ToString(),1),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Title.ToString(),2),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.PublicationType.ToString(),3),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Publisher.ToString(),4),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.YearRef.ToString(),5),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.IdRef.ToString(),6),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Education.ToString(),7),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Location.ToString(),8),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Semester.ToString(),9),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Language.ToString(),10),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.YearReport.ToString(),11),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.OriginalRef.ToString(),12),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Match.ToString(),13),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Comment.ToString(),14),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Syllabus.ToString(),15),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Season.ToString(),16),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.ExamEvent.ToString(),17),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Source.ToString(),18),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Pages.ToString(),19),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Volume.ToString(),20),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.Chapters.ToString(),21),
                new ColumnPositionHandler(Spreadsheet.ReferenceFields.BookTitle.ToString(),22),
        };
        [ObservableProperty]
        private bool isNextButtonEnabled;

        [ObservableProperty]
        private List<string> filePath;

        [ObservableProperty]
        private ObservableCollection<FileInfo> files;

        public DragAndDropViewModel()
        {
            Debug.WriteLine("DragAndDropView constructor Called");
            isNextButtonEnabled = false;
            files = new ObservableCollection<FileInfo>();
            //columnPos.CollectionChanged += ColumnPosChangedEventHandler;
        }
        partial void OnColumnPositionsChanged(ObservableCollection<ColumnPositionHandler> value)
        {
            Debug.WriteLine("ColumnPos changed");
            IsNextButtonEnabled = CanProceed();
        }

        //private void ColumnPosChangedEventHandler(object source, EventArgs e)
        //{
        //    Debug.WriteLine("Column Positions changed at DragAndDropView.");
        //    isNextButtonEnabled = CanProceed();
        //}

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
        private void AdjustColumnPositions() 
        {

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
            CloseWindow(window);
            WeakReferenceMessenger.Default.Send<FilePathsMessage>(new FilePathsMessage(files.ToList()));
        }
    }
}
