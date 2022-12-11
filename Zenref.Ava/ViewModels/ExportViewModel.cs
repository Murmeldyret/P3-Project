﻿using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;
using P3Project.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AvaloniaEdit;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using DynamicData.Binding;
using P3Project.API;
using Zenref.Ava.Models;
using Zenref.Ava.Models.Spreadsheet;
using Zenref.Ava.Views;
using Filter = Zenref.Ava.Models.Filter;

namespace Zenref.Ava.ViewModels
{
    public partial class ExportViewModel : ObservableRecipient, IRecipient<FilePathsMessage>
    {
        /// <summary>
        /// Property that keeps track of the number of references identified
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartCommand))]
        private int identifiedNumberCounter = 0;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartCommand))]
        private int unIdentifiedNumberCounter = 0;

        // Keeps track of which buttons are enabled
        [ObservableProperty] private bool isApiKeyButtonEnabled = true;
        [ObservableProperty] private bool isImportButtonEnabled = false;
        [ObservableProperty] private bool isStartButtonEnabled = false;
        [ObservableProperty] private bool isExportButtonEnabled = false;

        /// <summary>
        /// Makes a bool true
        /// </summary>
        /// <returns>true</returns>
        private bool canProceed()
        {
            return true;
        }
        /// <summary>
        /// Makes a bool false
        /// </summary>
        /// <returns>false</returns>
        private bool canNotProceed()
        {
            return false;
        }

        /// <summary>
        /// Property that hold the information from creating a new publicatino type
        /// </summary>
        /*
        private ObservableCollection<PublicationType> searchCriteria = new ObservableCollection<PublicationType>();
        private ObservableCollection<SearchPublicationType> searchTest = new ObservableCollection<SearchPublicationType>();
        private ObservableCollection<SearchPublicationType> searchTest2 = new ObservableCollection<SearchPublicationType>();
        */
        //private ObservableCollection<FilterType> searchCriteria = new ObservableCollection<FilterType>();
        private ObservableCollection<Filter> searchCriteria = new ObservableCollection<Filter>();
        //private List<Filter> searchCriteria = new List<Filter>();

        private ObservableCollection<SearchTerms> searchString = new ObservableCollection<SearchTerms>();

        /// <summary>
        /// A collection of the created publication types
        /// </summary>
        /*
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditPublicationTypeCommand))]
        private ObservableCollection<PublicationType> publicationTypes = new ObservableCollection<PublicationType>();
        */
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditPublicationTypeCommand))]
        private ObservableCollection<Filter> publicationTypes = new ObservableCollection<Filter>();
        //private ObservableCollection<FilterType> publicationTypes = new ObservableCollection<FilterType>();

        private List<Filter> ff = new List<Filter>();

        private List<FileInfo> filePaths;
        private List<int> columnPositions;
        private int activeSheet;
        [ObservableProperty] private ObservableCollection<Reference> references;
        [ObservableProperty] private ObservableCollection<RawReference> rawReferences;
        [ObservableProperty] private IEnumerable<Reference> filteredReferences;

        [ObservableProperty] private string apiKey;


        private int id = 0;

        /// <summary>
        /// Constructor, sets up the predefined publication types.
        /// And registers a message from the SearchCriteriaViewModel
        /// </summary>
        public ExportViewModel()
            : base(WeakReferenceMessenger.Default)
        {
            try
            {
                if (File.Exists(@"../../../Models/ApiKeys/scopusApiKey.txt"))
                {
                    using (StreamReader sr = new StreamReader(@"../../../Models/ApiKeys/scopusApiKey.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            IsImportButtonEnabled = canProceed();
                        }
                    }
                }
                else
                {
                    IsImportButtonEnabled = canNotProceed();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Messenger.Register<SearchTermMessage>(this, (r, m) =>
            {
                Receive(m);
            });

            Messenger.Register<FilePathsMessage>(this, (r, m) =>
            {
                Receive(m);
            });
        }
        public int DublicationCounter = 0;

        /// <summary>
        /// Handles the message sent from the SearchCriteriaViewModel.
        /// And adds the Criteria
        /// </summary>
        /// <param name="message"></param>
        public void Receive(SearchTermMessage message)
        {
            searchCriteria = message.SearchPubCollection;

            int pubId = 0;
            
            PublicationTypes.Add(new Filter(searchCriteria[0].filtQ, searchCriteria[0].filterQuery, $"Indsæt publikations titel"));
            /*
            if (PublicationTypes.Any())
            { 
                for (int i = 0; i < PublicationTypes.Count; i++)
                {
                    Console.WriteLine($"ID:{PublicationTypes[i].ListId}----------Name:{PublicationTypes[i].categoryName}");
                    if (PublicationTypes[i].categoryName == searchCriteria[0].categoryName)
                    {
                        //searchCriteria[0].categoryName = $"{searchCriteria[0].categoryName}1";
                        DublicationCounter++;
                        PublicationTypes.Add(new Filter(searchCriteria[0].filtQ, searchCriteria[0].filterQuery, $"{searchCriteria[0].categoryName}{DublicationCounter}"));
                    }

                    if (!PublicationTypes[i].categoryName.Contains(searchCriteria[0].categoryName))
                    {
                        Console.WriteLine("wow not on the list");
                    }
                }

                if (!PublicationTypes.Contains(searchCriteria[0]))
                {
                    Console.WriteLine("wavavaviva");
                    PublicationTypes.Add(new Filter(searchCriteria[0].filtQ, searchCriteria[0].filterQuery, searchCriteria[0].categoryName));
                }
            }
            else
            {
                PublicationTypes.Add(new Filter(searchCriteria[0].filtQ, searchCriteria[0].filterQuery, searchCriteria[0].categoryName));
            }
            */
        }

        [RelayCommand]
        private void OpenSearchCriteria(Window window)
        {
            bool isAddPubEnabled = true;
            SearchCriteriaView SearchView = new SearchCriteriaView();
            SearchView.DataContext = new SearchCriteriaViewModel(isAddPubEnabled);
            SearchView.Show();
        }

        /// <summary>
        /// Deletes the publication type with the same name as the one related to the delete button
        /// </summary>
        /// <param name="msg"></param>
        [RelayCommand]
        private void DeletePublicationType(object msg)
        {

            Console.WriteLine(msg.GetType().GetProperty("ListId").GetValue(msg));
            //int id = (int)msg.GetType().GetProperty("ListId").GetValue(msg);
            string name = (string)msg.GetType().GetProperty("categoryName").GetValue(msg);

            foreach (Filter f in PublicationTypes)
            {
                Console.WriteLine($"id: {f.ListId}");
            }
            
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (name == PublicationTypes[i].categoryName)
                {
                    PublicationTypes.RemoveAt(i);
                }
            }
        }

        [RelayCommand]
        private void OpenDragAndDropView(Window window)
        {
            DragAndDropView dragAndDropView = new DragAndDropView();
            dragAndDropView.ShowDialog(window);
            IsStartButtonEnabled = canProceed();
            IsApiKeyButtonEnabled = canNotProceed();
        }

        /// <summary>
        /// Edits a specific publication type.
        /// It opens a new window with the information related to the publication type
        /// </summary>
        /// <param name="msg"></param>
        [RelayCommand]
        private void EditPublicationType(object msg)
        {
            bool isEditEnabled = true;
            //int id = (int)msg.GetType().GetProperty("ListId").GetValue(msg);
            string name = (string)msg.GetType().GetProperty("categoryName").GetValue(msg);
            
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                //if (id == PublicationTypes[i].ListId)
                if (name == PublicationTypes[i].categoryName)
                {

                    PublicationTypes[i].cancel = false;
                    Console.WriteLine($"categoryName: {PublicationTypes[i].categoryName}");
                    //ff.Add(new Filter(PublicationTypes[i].filtQ, PublicationTypes[i].categoryName));
                    SearchCriteriaView SearchView = new SearchCriteriaView();
                    //SearchView.DataContext = new SearchCriteriaViewModel(ft[i].ftSearchTerm, msg, id);
                    //SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i].filtQ, PublicationTypes[i].categoryName, id);
                    //SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i], isEditEnabled);
                    SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i].filtQ, PublicationTypes[i].categoryName, isEditEnabled);
                    SearchView.Show();
                    Console.WriteLine($"pub::{PublicationTypes[i].categoryName}:::::::::::::se{searchCriteria[0].categoryName}");
                }
            }
            /*
            List<Filter> filterList = new List<Filter>();
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (msg == PublicationTypes[i].categoryName)
                {
                    filterList.Add(new Filter(PublicationTypes[i].filtQ, PublicationTypes[i].categoryName));
                    /*
                    for (int j = 0; j < PublicationTypes[i].filterQuery.Count; j++)
                    {
                        searchString.Add(new SearchTerms(PublicationTypes[i].filterQuery[j]));
                    }

                    PublicationTypes[i].cancel = false;
                    Console.WriteLine($"categoryName: {PublicationTypes[i].categoryName}");
                    ff.Add(new Filter(PublicationTypes[i].filtQ, PublicationTypes[i].categoryName));
                    SearchCriteriaView SearchView = new SearchCriteriaView();
                    //SearchView.DataContext = new SearchCriteriaViewModel(ft[i].ftSearchTerm, msg, id);
                    //SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i].filtQ, PublicationTypes[i].categoryName, id);
                    SearchView.DataContext =
                        new SearchCriteriaViewModel(PublicationTypes[i], PublicationTypes[i].cancel);
                    SearchView.Show();
                }
            }
        
                    */
            

            /*
            if (!searchString.Any())
            {
                for (int i = 0; i < PublicationTypes.Count; i++)
                {
                    for (int j = 0; j < PublicationTypes[i].filterQuery.Count; j++)
                    {
                        if (msg == PublicationTypes[i].categoryName)
                        {
                            searchString.Add(new SearchTerms(PublicationTypes[i].filterQuery[j]));
                            Console.WriteLine("hello from shit");
                                    
                        }
                    }
                }
            }
            */

            /*
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                foreach (string s in PublicationTypes[i].filterQuery)
                {
                    //Console.WriteLine($"edit btn from exportVM values: {s}");
                    searchString.Add(new SearchTerms(searchString: s));
                }

                //searchString = observableCollection<SearchTerm>
                // Publication[i].filterQuery = List<string>
                //searchString = PublicationTypes[i].filterQuery;
            }
            */
        }

        /// <summary>
        /// Adds api key to file - Creates file if it doesn't already exist
        /// </summary>
        [RelayCommand]
        private void AddApiKey()
        {
            string filename = @"../../../Models/ApiKeys/scopusApiKey.txt";

            try
            {
                // Check if the file already exists
                if (File.Exists(filename))
                {
                    // Overwrite the apikey txt file
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        sw.Write(ApiKey);
                    }
                    
                    // Read the apikey from file
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            ApiKey = line;
                        }
                    }
                }
                else
                {
                    // Creates new file
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        sw.WriteLine("Indsæt api nøgle");
                    }

                }

                IsStartButtonEnabled = canNotProceed();
                IsImportButtonEnabled = canProceed();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Starts to identify the references
        /// </summary>
        [RelayCommand]
        private void Start()
        {
            IdentifiedNumberCounter = 0;
            UnIdentifiedNumberCounter = 0;
/*
            // Read all the references from the excel file
            ReadAllReferences();

            // Identify the references in the database
            // TODO: Implement the identification of the references

            // If the database does not contain the reference, search for it in the internet
            ApiSearching apiSearching = new ApiSearching();
            // Call the apisearching method
            apiSearching.SearchReferences(rawReferences.ToList());
*/

            IsStartButtonEnabled = canNotProceed();
            IsExportButtonEnabled = canProceed();

        }

        /// <summary>
        /// Exports to excel
        /// </summary>
        [RelayCommand]
        private void Export()
        {
            Console.WriteLine("Export");
            IsApiKeyButtonEnabled = canProceed();
            IsExportButtonEnabled = false;
            IsStartButtonEnabled = false;
            IsImportButtonEnabled = false;
        }


        public void Receive(FilePathsMessage message)
        {
            filePaths = message.FilePaths;
            columnPositions = message.ColumnPositions;
            activeSheet = message.ActiveSheet;
            Debug.WriteLine("Received FilePathsMessage");
            Console.WriteLine("Filepaths: " + filePaths.Count);
            Console.WriteLine("ColumnPositions: " + columnPositions);
            Console.WriteLine("ActiveSheet: " + activeSheet);
        }

        private void ReadAllReferences()
        {
            ObservableCollection<RawReference> referencesInSheets = new ObservableCollection<RawReference>();
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
                    IEnumerable<RawReference> referencesInSheet = spreadsheet.GetReference(0u);
                    referencesInSheets.Add(referencesInSheet);
                }

                RawReferences = referencesInSheets;
                Debug.WriteLine($"Found {RawReferences.Count} Reference(s)");
            }
            catch (Exception e)
            {
                IMsBoxWindow<ButtonResult> messageBoxStandardView = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
                    .MessageBoxManager
                    .GetMessageBoxStandardWindow("Error", "Error in reading References from spreadsheet");
                messageBoxStandardView.Show();
                Debug.WriteLine("Error in reading references.");
                Debug.WriteLine(e.Message + e.StackTrace);
                Debug.WriteLine(positionInSheet.Count);
            }
            
        }
    }
}
