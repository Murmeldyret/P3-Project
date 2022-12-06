using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System;

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
        [ObservableProperty]
        private bool saveChanges = true;
        [ObservableProperty]
        private string[] propertyArray = { "Forfatter", "Titel", "Publikationstype", "Forlag", "År (Reference)", "Id", "Uddannelse", "Uddannelsessted", "Semester", "Sprog", "År (Rapport)", "Match", "Kommentar", "Pensum", "Sæson", "Eksamensbegivenhed", "Kilde", "Sidetal", "Bind", "Kapitler", "Bogtitel", "Henvisning" };

        public DatabaseViewModel() : base(WeakReferenceMessenger.Default)
        {
            Messenger.Register<FilePathsMessage>(this, (r,m) =>
            {
                Receive(m);
                // ReadAllReferences();
            });
            // FOR TESTING DATAGRID DISPLAYING REFERENCES
            //references = new ObservableCollection<Reference>();
            //RawReference rawReference = new RawReference("How to magic", "Hogwarts", "5. semester", "1234-4321", "Rowling, J. K. (1997). Harry Potter and the Philosopher’s Stone (1st ed.). Bloomsbury.");
            //references.Add(new Reference(rawReference, "J.K. Rowling", "Harry Potter and the Philosopher's Stone", "Bog", "Bloomsbury", 1997, "Engelsk", 2022, 0.8, "Kommentar", "How to wave a wand", "Forår", "Magic for beginners", "DanBib", 223, "Hvem ved", "Quidditch", "Bogtitel"));
            //for (int i = 0; i < 100; i++)
            //{
            //    List<string> s = new List<string>();
            //    for (int k = 0; k < 20; k++)
            //    {
            //        s.Add(RandomString(10));
            //    }
            //    double d = 0.5;
            //    RawReference rawReference1 = new RawReference(s[0], s[1], s[2], $"{i}", s[3]);
            //    references.Add(new Reference(rawReference1, s[4], s[5], s[6], s[7], i, s[8], i, d, s[9], s[10], s[11], s[12], s[13], i, s[14], s[15], s[16]));
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

        //FOR TESTING DATAGRID DISPLAYING REFERENCES
        //private static Random random = new Random();
        //public static string RandomString(int length)
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    return new string(Enumerable.Repeat(chars, length)
        //        .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
        [RelayCommand]
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

        /// <summary>
        /// Method called by button click. The method removes the <paramref name="selectedReference"/> from the collection of references.
        /// </summary>
        /// <param name="selectedReference">The reference selected in the datagrid</param>
        [RelayCommand]
        private async void DeleteReference(Reference selectedReference)
        {
            if (selectedReference != null)
            {
                IMsBoxWindow<string> messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                    new MessageBoxCustomParams
                    {
                        ContentTitle = "Slet reference",
                        ContentMessage = "Er du sikker på, at du vil slette referencen?",
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Nej", IsCancel = true },
                            new ButtonDefinition { Name = "Ja", IsDefault = true }
                        },
                    });
                if (await messageBoxCustomWindow.Show() == "Ja")
                {
                    references.Remove(selectedReference);
                }
            }
        }
        /// <summary>
        /// Event raised by closing of the window. 
        /// </summary>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Code to be executed before window is closed.
        }
    }
}
