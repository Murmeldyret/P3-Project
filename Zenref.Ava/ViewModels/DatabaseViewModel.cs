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
using MessageBox.Avalonia.Enums;
using System;
using System.Reactive.Linq;

namespace Zenref.Ava.ViewModels
{
    public partial class DatabaseViewModel : ObservableRecipient, IRecipient<FilePathsMessage>
    {

        [ObservableProperty]
        private ObservableCollection<Reference> references;
        [ObservableProperty]
        private ObservableCollection<Reference> inputReferences = new ObservableCollection<Reference>();
        [ObservableProperty]
        private IEnumerable<Reference> filteredReferences;
        private List<FileInfo> filePaths;
        private List<int> columnPositions;
        private int activeSheet;
        [ObservableProperty]
        private bool saveChanges = true;
        [ObservableProperty]
        private string[] propertyArray = { "Id", "Forfatter", "Titel", "Publikationstype", "Forlag", "År (Reference)", "Reference id", "Uddannelse", "Uddannelsessted", "Semester", "Sprog", "År (Rapport)", "Match", "Kommentar", "Pensum", "Sæson", "Eksamensbegivenhed", "Kilde", "Sidetal", "Bind", "Kapitler", "Bogtitel", "Henvisning" };

        public DatabaseViewModel() : base(WeakReferenceMessenger.Default)
        {
            Messenger.Register<FilePathsMessage>(this, (r,m) =>
            {
                Receive(m);
                ReadAllReferences();
                foreach (Reference reference in inputReferences)
                {
                    references.Add(reference);
                }
            });
            using (var context = new DataContext())
            {
                var referenceList = context.References.ToList();
                references = new ObservableCollection<Reference>(referenceList);
            }
            filteredReferences = references;
        }

        public void Receive(FilePathsMessage message)
        {
            filePaths = message.FilePaths;
            columnPositions = message.ColumnPositions;
            activeSheet = message.ActiveSheet;
            Debug.WriteLine("Received FilepathsMessage.");
        }

        [RelayCommand]
        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
        }

        private void ReadAllReferences()
        {
            ObservableCollection<Reference> referencesInSheets = new ObservableCollection<Reference>();
            Dictionary<Spreadsheet.ReferenceFields, int> positionInSheet = new Dictionary<Spreadsheet.ReferenceFields, int>();
            Spreadsheet.ReferenceFields referenceFields = (Spreadsheet.ReferenceFields)0;
            for (int i = 0; i < columnPositions.Count; i++)
            {
                positionInSheet.Add(referenceFields++, columnPositions[i]);
            }

            try
            {
                foreach (FileInfo path in filePaths)
                {
                    Spreadsheet spreadsheet = new Spreadsheet(path.FullName);
                    Debug.WriteLine($"FileName: {path.Name} Path: {path.DirectoryName}");
                    spreadsheet.SetColumnPosition(positionInSheet);
                    spreadsheet.Import();
                    spreadsheet.SetActiveSheet(activeSheet);
                    Debug.WriteLine($"SPREADSHEET count: {spreadsheet.Count}");
                    IEnumerable<Reference> referencesInSheet = spreadsheet.GetReference(0u);
                    referencesInSheets.Add(referencesInSheet);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in reading references.");
                Debug.WriteLine(e.Message + e.StackTrace);
                Debug.WriteLine(positionInSheet.Count);
            }
            finally
            {
                int nullReferences = referencesInSheets.Where(x => x is null).Count();
                if (nullReferences > 0)
                {
                    IMsBoxWindow<ButtonResult> messageBoxStandardView = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
                        .MessageBoxManager
                        .GetMessageBoxStandardWindow("Error", $"Failed to read {nullReferences} reference(s)");
                    messageBoxStandardView.Show();   
                }
                inputReferences = new ObservableCollection<Reference>();
                InputReferences.AddRange(referencesInSheets.Where(x => x is not null));
                Debug.WriteLine($"Found {InputReferences.Count} Reference(s)");
                Debug.WriteLine($"Number of failed references: {nullReferences}");   
            }
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
                            new ButtonDefinition { Name = "Ja", IsDefault = true },
                            new ButtonDefinition { Name = "Nej", IsCancel = true }
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
            using (var context = new DataContext())
            {
                if (saveChanges)
                {
                    foreach (Reference reference in context.References)
                    {
                        context.References.Remove(reference);
                    }
                    foreach (Reference reference in references)
                    {
                        context.References.Add(reference);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
