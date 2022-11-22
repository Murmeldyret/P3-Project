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

namespace Zenref.Ava.ViewModels
{
    public partial class DatabaseViewModel : ObservableRecipient, IRecipient<FilePathsMessage>
    {

        [ObservableProperty]
        private ObservableCollection<Reference> references;
        [ObservableProperty]
        private IEnumerable<Reference> filteredReferences;
        private List<FileInfo> filePaths;

        public DatabaseViewModel() : base(WeakReferenceMessenger.Default)
        {
            Messenger.Register<FilePathsMessage>(this, (r,m) =>
            {
                Receive(m);
                ReadAllReferences();
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
            Debug.WriteLine(filePaths[0]);
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
            //TODO get dragAndDrop filepath strings
            List<Reference> references = new List<Reference>();

            foreach (FileInfo path in filePaths)
            {
                
                Spreadsheet spreadsheet = new Spreadsheet(path.Name,path.DirectoryName);
                Debug.WriteLine($"FileName: {path.Name} Path: {path.DirectoryName}");
                spreadsheet.Import();
                Debug.WriteLine($"SPREADSHEET count: {spreadsheet.Count}");
                IEnumerable<Reference> referencesInSheet = spreadsheet.GetReference(0u);
                references.Add(referencesInSheet);
                references.Concat(referencesInSheet);
            }
            Debug.WriteLine(references.Count);

        }

    }
}
