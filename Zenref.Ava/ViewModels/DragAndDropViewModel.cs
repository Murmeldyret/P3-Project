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
        [ObservableProperty]
        private bool isNextButtonEnabled;

        [ObservableProperty]
        private List<string> filePath;

        [ObservableProperty]
        private ObservableCollection<FileInfo> files;

        private HashSet<int> position = new HashSet<int>();
        [ObservableProperty]
        private ObservableCollection<KeyValuePair<Spreadsheet.ReferenceFields, int>> columnPositions = new ObservableCollection<KeyValuePair<Spreadsheet.ReferenceFields, int>>()
        {
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Author, 1 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Title, 2 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.PublicationType, 3 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Publisher, 4 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.YearRef, 5 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.IdRef, 6 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Education, 7 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Location, 8 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Semester, 9 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Language, 10 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.YearReport, 11 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.OriginalRef, 12 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Match, 13 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Comment, 14 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Syllabus, 15 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Season, 16 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.ExamEvent, 17 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Source, 18 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Pages, 19 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Volume, 20 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.Chapters, 21 )},
            { new KeyValuePair<Spreadsheet.ReferenceFields,int>( Spreadsheet.ReferenceFields.BookTitle, 22 )},
        };
        [ObservableProperty]
        private ObservableCollection<string> columnName = new ObservableCollection<string>()
        {
            Spreadsheet.ReferenceFields.Author.ToString(),
            Spreadsheet.ReferenceFields.Title.ToString(),
            Spreadsheet.ReferenceFields.PublicationType.ToString(),
            Spreadsheet.ReferenceFields.Publisher.ToString(),
            Spreadsheet.ReferenceFields.YearRef.ToString(),
            Spreadsheet.ReferenceFields.IdRef.ToString(),
            Spreadsheet.ReferenceFields.Education.ToString(),
            Spreadsheet.ReferenceFields.Location.ToString(),
            Spreadsheet.ReferenceFields.Semester.ToString(),
            Spreadsheet.ReferenceFields.Language.ToString(),
            Spreadsheet.ReferenceFields.YearReport.ToString(),
            Spreadsheet.ReferenceFields.OriginalRef.ToString(),
            Spreadsheet.ReferenceFields.Match.ToString(),
            Spreadsheet.ReferenceFields.Comment.ToString(),
            Spreadsheet.ReferenceFields.Syllabus.ToString(),
            Spreadsheet.ReferenceFields.Season.ToString(),
            Spreadsheet.ReferenceFields.ExamEvent.ToString(),
            Spreadsheet.ReferenceFields.Source.ToString(),
            Spreadsheet.ReferenceFields.Pages.ToString(),
            Spreadsheet.ReferenceFields.Volume.ToString(),
            Spreadsheet.ReferenceFields.Chapters.ToString(),
            Spreadsheet.ReferenceFields.BookTitle.ToString(),
        };
        [ObservableProperty]
        private ObservableCollection<int> columnPos = new ObservableCollection<int>()
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,

        };
        public DragAndDropViewModel()
        {
            Debug.WriteLine("DragAndDropView constructor Called");
            isNextButtonEnabled = false;
            files = new ObservableCollection<FileInfo>();
            columnPos.CollectionChanged += ColumnPosChangedEventHandler;
        }

        private void ColumnPosChangedEventHandler(object source, EventArgs e)
        {
            Debug.WriteLine("Column Positions changed at DragAndDropView.");
            isNextButtonEnabled = CanProceed();
        }

        /// <summary>
        /// Determines whether or not the user can press the next button by checking that any file is chosen, and that the column position is correctly filled
        /// </summary>
        /// <returns>True if the user can proceed, false otherwise</returns>
        private bool CanProceed()
        {
            int distinctColumnPosValues = columnPos.Where(x => x is > 0 and <= 22).Distinct().Count();

            bool isColumnPosFilled = distinctColumnPosValues == columnPos.Count;
            bool anyFileChosen = files.Count() != 0;

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
