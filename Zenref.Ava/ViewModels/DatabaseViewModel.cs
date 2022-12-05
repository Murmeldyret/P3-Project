using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenref.Ava.Models;
using Zenref.Ava.Views;
using System.Linq;
using Zenref.Ava.Models.Spreadsheet;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.IO;
using DynamicData;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;

namespace Zenref.Ava.ViewModels
{
    public partial class DatabaseViewModel : ObservableRecipient, IRecipient<FilePathsMessage>
    {

        [ObservableProperty]
        private ObservableCollection<Reference> references;
        [ObservableProperty] 
        private ObservableCollection<RawReference> rawReferences;
        [ObservableProperty]
        private IEnumerable<Reference> filteredReferences;
        private List<FileInfo> filePaths;
        private List<int> columnPositions;
        private int activeSheet;

        public DatabaseViewModel() : base(WeakReferenceMessenger.Default)
        {
            Messenger.Register<FilePathsMessage>(this, (r,m) =>
            {
                Receive(m);
                // ReadAllReferences();
            });
            // FOR TESTING DATAGRID DISPLAYING REFERENCES
            //references = new ObservableCollection<Reference>();
            //for (int i = 0; i < 15; i++)
            //{
            //    List<string> s = new List<string>();
            //    for (int k = 0; k < 20; k++)
            //    {
            //        s.Add(RandomString(10));
            //    }
            //    double d = 0.5;
            //    references.Add(new Reference(s[0], s[1], s[2], s[3], i, i, s[4], s[5], s[6], s[7], i, d, s[8], s[9], s[10], s[11], s[12], i, s[13], s[14], s[15], s[16]));
            //}
            //filteredReferences = references;
        }

        public void Receive(FilePathsMessage message)
        {
            filePaths = message.FilePaths;
            columnPositions = message.ColumnPositions;
            activeSheet = message.ActiveSheet;
            Debug.WriteLine("Received FilepathsMessage.");
        }

        // FOR TESTING DATAGRID DISPLAYING REFERENCES
        //private static Random random = new Random();
        //public static string RandomString(int length)
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    return new string(Enumerable.Repeat(chars, length)
        //        .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
        }

        private void ReadAllReferences()
        {
        //     ObservableCollection<RawReference> referencesInSheets = new ObservableCollection<RawReference>();
        //     SortedDictionary<Spreadsheet.ReferenceFields, int> positionInSheet = new SortedDictionary<Spreadsheet.ReferenceFields, int>();
        //     Spreadsheet.ReferenceFields referenceFields = (Spreadsheet.ReferenceFields)0;
        //     for (int i = 0; i < columnPositions.Count; i++)
        //     {
        //         positionInSheet.Add(referenceFields++,columnPositions[i]);
        //     }
        //
        //     try
        //     {
        //         foreach (FileInfo path in filePaths)
        //         {
        //
        //             Spreadsheet spreadsheet = new Spreadsheet(path.Name, path.DirectoryName);
        //             Debug.WriteLine($"FileName: {path.Name} Path: {path.DirectoryName}");
        //             spreadsheet.SetColumnPosition(positionInSheet);
        //             spreadsheet.Import();
        //             spreadsheet.SetActiveSheet(activeSheet);
        //             Debug.WriteLine($"SPREADSHEET count: {spreadsheet.Count}");
        //             IEnumerable<RawReference> referencesInSheet = spreadsheet.GetReference(0u);
        //             referencesInSheets.Add(referencesInSheet);
        //         }
        //         RawReferences = referencesInSheets;
        //         Debug.WriteLine($"Found {references.Count} Reference(s)");
        //     }
        //     catch (Exception e)
        //     {
        //         IMsBoxWindow<ButtonResult> messageBoxStandardView = MessageBox.Avalonia.MessageBoxManager
        //             .GetMessageBoxStandardWindow("Error", "Error in reading References from spreadsheet");
        //         messageBoxStandardView.Show();
        //         Debug.WriteLine("Error in reading references.");
        //     }
        }

    }
}
