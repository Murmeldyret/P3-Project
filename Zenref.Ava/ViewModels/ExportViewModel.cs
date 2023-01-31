using Avalonia;
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
using System.ComponentModel;


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
        [ObservableProperty]
        private int totalReferences = 0;

        // Keeps track of which buttons are enabled
        [ObservableProperty] private bool isApiKeyButtonEnabled = true;
        [ObservableProperty] private bool isImportButtonEnabled = false;
        [ObservableProperty] private bool isStartButtonEnabled = false;
        [ObservableProperty] private bool isExportButtonEnabled = false;
        [ObservableProperty] private bool isSaveFilterButtonEnabled = false;

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
        private ObservableCollection<Filter> searchCriteria = new ObservableCollection<Filter>();

        private FilterCollection PremadeFilter = IFilterCollection.GetInstance();
                
        /// <summary>
        /// A collection of the created publication types
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditPublicationTypeCommand))]
        private ObservableCollection<Filter> publicationTypes = new ObservableCollection<Filter>();


        private List<FileInfo> filePaths;
        private List<int> columnPositions;
        private int activeSheet;
        [ObservableProperty] private ObservableCollection<Reference> references;
        [ObservableProperty] private ObservableCollection<RawReference> rawReferences;
        [ObservableProperty] private IEnumerable<Reference> filteredReferences;
        [ObservableProperty] private string apiKey;

        private BackgroundWorker StartWorker;
        private bool _isRunning;


        [RelayCommand]
        private void SaveFilter()
        { 
            // : Clear the premade filter collection
            PremadeFilter.Clear();
            
            // : Check if there are any publication types
            if (PublicationTypes.Any())
            {
                // : Loop through each publication type
                foreach (Filter f in PublicationTypes)
                {
                    // : Add the publication type to the premade filter collection
                    PremadeFilter.Add(f);
                    Console.WriteLine($"filtC:{PremadeFilter}");
                }
                
                // : Save the filters
                PremadeFilter.SaveFilters();
                
                // : Show a message box indicating that the publication types have been saved
                IMsBoxWindow<ButtonResult> messageSaveFilterBox = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
                    .MessageBoxManager
                    .GetMessageBoxStandardWindow("Filter Gemt", "Publikationstyperne er blevet gemt\n til næste gang programmet starter");
                messageSaveFilterBox.Show();
                
                // : Set the save filter button to be enabled
                IsSaveFilterButtonEnabled = true;
            }
        }


        /// <summary>
        /// Constructor, sets up the predefined publication types.
        /// And registers a message from the SearchCriteriaViewModel
        /// </summary>
        public ExportViewModel()
            : base(WeakReferenceMessenger.Default)
        {
            // Searching for api key
            try
            {
                // : Check if the "ApiKeys" directory exists
                if (!Directory.Exists(@"./ApiKeys"))
                {
                    Console.WriteLine("directory create");
                    
                    // : If not, create it
                    Directory.CreateDirectory(@"./ApiKeys");
                }
                else
                {
                    // : If the directory exists, check if the scopusApiKey.txt file exists
                    if (File.Exists(@"./ApiKeys/scopusApiKey.txt"))

                    {
                        using (StreamReader sr = new StreamReader(@"./ApiKeys/scopusApiKey.txt"))
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
                
                // : Load the premade filters if any exist
                if (PremadeFilter.Any())
                {
                    // : Load the filters from the PremadeFilter collection
                    PremadeFilter.LoadFilters();
                    
                    // : Counter variable for tracking the number of items processed in the loop
                    int inc = 0;
                    
                    // : Loop through each filter in the PremadeFilter collection
                    foreach (Filter filter in PremadeFilter)
                    {
                        // : Call the ReturnFilterCategory method for the current filter
                        filter.ReturnFilterCategory();
                        
                        // : Add the current filter to the PublicationTypes collection
                        PublicationTypes.Add(filter);
                        
                        // : Loop through each item in the PublicationTypes collection starting from the inc value
                        for (int i = inc; i < PublicationTypes.Count; i++)
                        {
                            // : Create a new ObservableCollection of SearchTerms for the filtQ property of the current item in PublicationTypes
                            PublicationTypes[i].filtQ = new ObservableCollection<SearchTerms>();
                            
                            // : Loop through each query in the filter
                            foreach (string query in filter)
                            {
                                // : Add a new SearchTerms object to the filtQ property with the current query
                                PublicationTypes[i].filtQ.Add(new SearchTerms(query));
                            }
                            
                        }
                        // : Increment the inc counter
                        inc++;

                    }
                    // : Set the IsSaveFilterButtonEnabled property to true
                    IsSaveFilterButtonEnabled = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            // : Register a message from the SearchCriteriaViewModel
            Messenger.Register<SearchTermMessage>(this, (r, m) =>
            {
                Receive(m);
            });
            
            // : Register another message from the SearchCriteriaViewModel
            Messenger.Register<FilePathsMessage>(this, (r, m) =>
            {
                Receive(m);
            });
        }

        /// <summary>
        /// Handles the message sent from the SearchCriteriaViewModel.
        /// And adds the Criteria
        /// </summary>
        /// <param name="message"></param>
        public void Receive(SearchTermMessage message)
        {
            // : Store the search criteria sent from the SearchCriteriaViewModel
            searchCriteria = message.SearchPubCollection;
            
            // : Add the filter object to the PublicationTypes collection
            PublicationTypes.Add(new Filter(searchCriteria[0].filtQ, searchCriteria[0].filterQuery, $"Titel"));
            
            // : Enable/disable the SaveFilter button based on the PublicationTypes collection being empty or not
            if (PublicationTypes.Any())
            {
                IsSaveFilterButtonEnabled = canProceed();
            }
            else
            {
                IsSaveFilterButtonEnabled = canNotProceed();
            }
        }

        /// <summary>
        /// Opens the SearchCritetiaView, so the user can add a publication type
        /// : Handles the opening of the SearchCriteriaView so the user can add a publication type.
        /// </summary>
        [RelayCommand]
        private void OpenSearchCriteria()
        {
            // : Flag to enable adding a publication type
            bool isAddPubEnabled = true;
            
            // : Instance of the SearchCriteriaView
            SearchCriteriaView SearchView = new SearchCriteriaView();
            
            // : Sets the DataContext of the SearchCriteriaView to a new instance of the SearchCriteriaViewModel with the isAddPubEnabled flag
            SearchView.DataContext = new SearchCriteriaViewModel(isAddPubEnabled);
            
            // : Shows the SearchCriteriaView
            SearchView.Show();
        }

        /// <summary>
        /// Deletes the publication type with the same name as the one related to the delete button
        /// </summary>
        /// <param name="msg">: Object that contains information about the publication type to be deleted</param>
        [RelayCommand]
        private void DeletePublicationType(object msg)
        {
            // : Get the category name of the publication type to be deleted
            string name = (string)msg.GetType().GetProperty("categoryName").GetValue(msg);

            // Find the name of the publication type and delete it
            // : Loop through the list of publication types
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                // : If the category name of the current publication type matches the name of the publication type to be deleted
                if (name == PublicationTypes[i].categoryName)
                {
                    // : Remove the publication type from the list
                    PublicationTypes.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Opens drag and drop view to import excel file
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        private void OpenDragAndDropView(Window window)
        {
            // : create a new instance of the DragAndDropView
            DragAndDropView dragAndDropView = new DragAndDropView();
            
            // : show the drag and drop view as a modal dialog, with the specified parent window
            dragAndDropView.ShowDialog(window);
            
            // : update the state of the "ApiKeyButtonEnabled" and "StartButtonEnabled" properties based on the results of the drag and drop operation
            IsApiKeyButtonEnabled = canNotProceed();
            IsStartButtonEnabled = canProceed();
        }

        /// <summary>
        /// Edits a specific publication type.
        /// It opens a new window with the information related to the publication type
        /// </summary>
        /// <param name="msg"></param>
        [RelayCommand]
        private void EditPublicationType(object msg)
        {
            // : flag to indicate that it is editing
            bool isEditEnabled = true;
            
            // : get the name of the publication type to be edited
            string name = (string)msg.GetType().GetProperty("categoryName").GetValue(msg);

            // Loop over publication types
            // : loop through the publication types list
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                // Take the chosen publication type and open it in SearchCriteriaView for edit
                // : find the publication type with the same name as the one to be edited
                if (name == PublicationTypes[i].categoryName)
                {
                    // : set cancel flag to false
                    PublicationTypes[i].cancel = false;
                    
                    // : create instance of SearchCriteriaView
                    SearchCriteriaView SearchView = new SearchCriteriaView();
                    
                    // : set the data context to an instance of SearchCriteriaViewModel with the publication type to be edited and the isEditEnabled flag
                    SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i], isEditEnabled);
                    
                    // : show the SearchCriteriaView
                    SearchView.Show();
                }
            }
        }

        /// <summary>
        /// Adds api key to file - Creates file if it doesn't already exist
        /// </summary>
        [RelayCommand]
        private void AddApiKey()
        {
            string filename = @"./ApiKeys/scopusApiKey.txt";

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
                        sw.Write(ApiKey);
                    }

                }
                
                // : Display a success message to the user
                IMsBoxWindow<ButtonResult> messageSaveFilterBox = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
                    .MessageBoxManager
                    .GetMessageBoxStandardWindow("API nøgle", "API nøglen er blevet gemt");
                messageSaveFilterBox.Show();
                
                // : Disable the start button and enable the import button
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


            IsStartButtonEnabled = canNotProceed();
            
            // : Initializes the background worker 
            // : A BackgroundWorker is a component in the .NET Framework that allows to run a time-consuming task in a separate thread, so that the user interface remains responsive
            StartWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            
            // : This event is where you place the code for the task you want to run in the background. The code in this event should not interact with the user interface.
            StartWorker.DoWork += RunBackgroundSearchProcess;
            
            // : This event is raised when the ReportProgress method is called from the DoWork event. It can be used to update the user interface with progress information while the task is running.
            StartWorker.ProgressChanged += ChangedBackgroundSearchProcess;
            
            // : This event is raised when the background task is completed. It can be used to update the user interface with the results of the task.
            StartWorker.RunWorkerCompleted += CompletedBackgroundSearchProcess;

            StartWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Exports references to an excel workbook.
        /// </summary>
        /// <param name="sheet">The sheet to write to</param>
        /// <param name="name">The name of the outputted Excel file (including file suffix)</param>
        /// <remarks>
        /// Groups references by their publication type where each group will be saved to a separate worksheet.
        /// </remarks>
        private void Export(Spreadsheet sheet, string name)
        {
            // : Disable buttons during export
            IsApiKeyButtonEnabled = canProceed();
            IsExportButtonEnabled = false;
            IsStartButtonEnabled = false;
            IsImportButtonEnabled = false;
            
            // : Group references by pubType
            IEnumerable<IGrouping<string, Reference>> referencesGroupedByPubType = filteredReferences.GroupBy(
                reference => reference.PubType.ToLower());
            
            // : Iterate over each group and add the references to the sheet for the corresponding pubType
            foreach (IGrouping<string, Reference> grouping in referencesGroupedByPubType)
            {
                Debug.WriteLine($"{grouping.Key} has {grouping.Count()} reference(s)");
                sheet.SetActiveSheet(grouping.Key);
                sheet.AddReference(grouping);
            }
            
            // : Log the number of references exported
            Debug.WriteLine($"testreferences count:{FilteredReferences.Count()}");
            sheet.Export(name);
            Debug.WriteLine($"Exported {filteredReferences.Count()} Reference(s).");
        }
        /// <summary>
        /// Prompts the user to save a file at a given location
        /// </summary>
        /// <param name="window">The window that creates the dialog</param>
        [RelayCommand]
        private async void SaveFileDialog(Window window)
        {
            // ReadAllReferences();
            // : Create a save file dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Choose export folder",
                InitialFileName = "Behandlede_Referencer.xlsx",
                DefaultExtension = ".xlsx",

            };
            
            // : Show the save file dialog and get the file path
            string? filePathToExportedFile = await saveFileDialog.ShowAsync(window);
            
            // : Check if the file path is null
            if (filePathToExportedFile is null)
            {
                // : Show an error message box if the file path is null
                IMsBoxWindow<ButtonResult> messageBoxStandardView = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Error", "Error in reading References from spreadsheet");
                messageBoxStandardView.Show();
            }
            else
            {
                // : Create a new spreadsheet
                Spreadsheet exportSheet = new Spreadsheet(filePathToExportedFile);
                exportSheet.Create();
                
                // : Call the Export method
                Export(exportSheet, filePathToExportedFile);
            }
        }


        /// <summary>
        /// Method called when this class receives a message on the default channel of type <c>FilePathsMessage.</c>
        /// </summary>
        /// <param name="message">The message received.</param>
        public void Receive(FilePathsMessage message)
        {
            // : Store the received file paths and column positions in instance variables.
            filePaths = message.FilePaths;
            columnPositions = message.ColumnPositions;
            activeSheet = message.ActiveSheet;
            
            // : Log receipt of the message.
            Debug.WriteLine("Received FilePathsMessage");
            Console.WriteLine("Filepaths: " + filePaths.Count);
            Console.WriteLine("ColumnPositions: " + columnPositions);
            Console.WriteLine("ActiveSheet: " + activeSheet);
            
            // : Call the `ReadAllReferences` method to process the received data.
            ReadAllReferences();
        }

        /// <summary>
        /// Reads all the references from the chosen Excel files.
        /// </summary>
        private void ReadAllReferences()
        {
            // : An ObservableCollection to store the references in the sheets
            ObservableCollection<RawReference> referencesInSheets = new ObservableCollection<RawReference>();
            
            // : A dictionary to store the position of each reference field in the sheet
            Dictionary<Spreadsheet.ReferenceFields, int> positionInSheet = new Dictionary<Spreadsheet.ReferenceFields, int>();
            Spreadsheet.ReferenceFields referenceFields = (Spreadsheet.ReferenceFields)0;
            
            // : Loop through the column positions and add them to the dictionary
            for (int i = 0; i < columnPositions.Count; i++)
            {
                positionInSheet.Add(referenceFields++, columnPositions[i]);
            }

            try
            {
                // : Loop through the file paths
                foreach (FileInfo path in filePaths)
                {
                    // :Create a Spreadsheet object for the file
                    Spreadsheet spreadsheet = new Spreadsheet(path.FullName);
                    Debug.WriteLine($"FileName: {path.Name} Path: {path.DirectoryName}");
                    
                    // : Set the column positions for the Spreadsheet object
                    spreadsheet.SetColumnPosition(positionInSheet);
                    
                    // :Import the data from the file
                    spreadsheet.Import();
                    
                    // :Set the active sheet for the Spreadsheet object
                    spreadsheet.SetActiveSheet(activeSheet);
                    Debug.WriteLine($"SPREADSHEET count: {spreadsheet.Count}");
                    
                    // :Get the references from the Spreadsheet object
                    IEnumerable<RawReference> referencesInSheet = spreadsheet.GetReference(0u);
                    
                    // : Add the references to the referencesInSheets collection
                    referencesInSheets.Add(referencesInSheet);
                }
                
                // : Set the RawReferences property to the referencesInSheets collection
                RawReferences = referencesInSheets;
                Debug.WriteLine($"Found {RawReferences.Count} Reference(s)");
            }
            catch (Exception e)
            {
                // : If there is an error in reading the references, display a message box with the error message
                IMsBoxWindow<ButtonResult> messageBoxStandardView = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
                    .MessageBoxManager
                    .GetMessageBoxStandardWindow("Error", "Error in reading References from spreadsheet");
                messageBoxStandardView.Show();
                Debug.WriteLine("Error in reading references.");
                Debug.WriteLine(e.Message + e.StackTrace);
                Debug.WriteLine(positionInSheet.Count);
            }
        }

        /// <summary>
        /// The background process to search the references from the database.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event argument that holds the data for the event.</param>
        private void RunBackgroundSearchProcess(object sender, DoWorkEventArgs e)
        {
            // : Flag to indicate the running status of the background process
            _isRunning = true;
            
            // : Initialize the reference count
            int[] countRef = { 0, 0 };
            
            // Read all the references from the excel file
            // : Get the total number of references
            TotalReferences = rawReferences.Count;
            
            // : Initialize the lists to store the filtered and unfiltered references
            List<Reference> OverAllReferences = new List<Reference>();
            List<RawReference> leftOver = new List<RawReference>();

            // Filter the references
            // : Get the instance of the filter collection
            FilterCollection instance = IFilterCollection.GetInstance();

            // : Loop through each raw reference
            foreach (RawReference reference in rawReferences.ToList())
            {
                // : Get the reference from the database
                Reference dbReference = DatabaseHelper.GetReference(reference.ExtractData().Title);
                
                // : If the reference is found in the database
                if (dbReference.Title != "")
                {
                    // : Add the reference to the filtered reference list
                    OverAllReferences.Add(dbReference);
                }
                else
                {
                    // : Add the reference to the unfiltered reference list
                    leftOver.Add(reference);
                }
            }



            // If the database does not contain the reference, search for it in the internet
            // : Search for references not found in the database in the internet
            ApiSearching apiSearching = new ApiSearching();
            
            // Call the apisearching method
            (List<Reference> listReferences, leftOver) = apiSearching.SearchReferences(leftOver);

            Console.WriteLine("References: " + rawReferences.Count);
            
            // : Add the references found in the internet to OverAllReferences
            foreach (Reference reference in listReferences)
            {
                OverAllReferences.Add(reference);
            }

            // Categorize all the references
            // : Categorize all the references in OverAllReferences
            foreach (Reference reference in OverAllReferences)
            {
                // : Update the reference count
                UpdateCounter(instance, countRef, reference);
                
                // : Report progress to the background worker
                StartWorker.ReportProgress(0, countRef);
            }

            // Categorize all the remaining raw references
            // : Categorize all the remaining raw references in leftOver
            foreach (RawReference rawreference in leftOver)
            {
                Reference reference = rawreference.ExtractData();
                
                // : Categorize the reference
                reference.PubType = instance.categorize(reference);
                
                // : Update the reference count
                UpdateCounter(instance, countRef, reference);
                
                // : Add the categorized reference to OverAllReferences
                OverAllReferences.Add(reference);
                
                // : Report progress to the background worker
                StartWorker.ReportProgress(0, countRef);
            }
            
            // : Store the final list of references
            FilteredReferences = OverAllReferences;
        }
        
        // UpdateCounter method is used to update the counter of the references categorized and uncategorized.
        private static void UpdateCounter(FilterCollection instance, int[] countRef, Reference reference)
        {
            // Call the categorize function.
            // : Check if the reference has not been categorized yet.
            if (reference.PubType == null)
            {
                // : Call the categorize function of the FilterCollection instance to categorize the reference.
                reference.PubType = instance.categorize(reference);
            }
            
            // : Increment the count of the categorized references.
            if (reference.PubType != "Uncategorized")
            {
                countRef[0]++;
            }
            // : Increment the count of the uncategorized references.
            else
            {
                countRef[1]++;
            }
        }

        // : CompletedBackgroundSearchProcess function is called when the background search process is completed
        private void CompletedBackgroundSearchProcess(object sender, RunWorkerCompletedEventArgs e)
        {
            // : Set the running status to false
            _isRunning = false;
            
            // : Set the export button as enabled
            IsExportButtonEnabled = true;
            
            // : Show a message box to notify the user that the search process is completed
            IMsBoxWindow<ButtonResult> messageSaveFilterBox = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
.MessageBoxManager.GetMessageBoxStandardWindow("Identifikations process", "Identifikations processen er blevet gennemført.");
            messageSaveFilterBox.Show();
        }

        // : ChangedBackgroundSearchProcess function is called when the background search process reports a progress change
        private void ChangedBackgroundSearchProcess(object sender, ProgressChangedEventArgs e)
        {
            // : Get the count of references
            int[] countRef = e.UserState as int[];
            
            // : Update the count of identified references
            IdentifiedNumberCounter = countRef![0];
            
            // : Update the count of un-identified references
            UnIdentifiedNumberCounter = countRef[1];
        }
    }
}
