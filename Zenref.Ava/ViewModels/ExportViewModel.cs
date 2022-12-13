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


        private ObservableCollection<(Filter filter, int id)> filtercollection = new ObservableCollection<(Filter, int)>();
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
                if (File.Exists(@"ApiKeys/scopusApiKey.txt"))
                {
                    using (StreamReader sr = new StreamReader(@"ApiKeys/scopusApiKey.txt"))
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

        /// <summary>
        /// Handles the message sent from the SearchCriteriaViewModel.
        /// And adds the Criteria
        /// </summary>
        /// <param name="message"></param>
        public void Receive(SearchTermMessage message)
        {
            searchCriteria = message.SearchPubCollection;

            PublicationTypes.Add(new Filter(searchCriteria[0].filtQ, searchCriteria[0].filterQuery, $"Titel"));
        }

        /// <summary>
        /// Opens the SearchCritetiaView, so the user can add a publication type
        /// </summary>
        [RelayCommand]
        private void OpenSearchCriteria()
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

            string name = (string)msg.GetType().GetProperty("categoryName").GetValue(msg);

            // Find the name of the publication type and delete it
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (name == PublicationTypes[i].categoryName)
                {
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
            string name = (string)msg.GetType().GetProperty("categoryName").GetValue(msg);

            // Loop over publication types
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                // Take the chosen publication type and open it in SearchCriteriaView for edit
                if (name == PublicationTypes[i].categoryName)
                {
                    PublicationTypes[i].cancel = false;
                    SearchCriteriaView SearchView = new SearchCriteriaView();
                    SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i], isEditEnabled);
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


            IsStartButtonEnabled = canNotProceed();

            StartWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            StartWorker.DoWork += RunBackgroundSearchProcess;
            StartWorker.ProgressChanged += ChangedBackgroundSearchProcess;
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
            // List<Reference> testreferences = new List<Reference>();
            // foreach (RawReference reference in rawReferences)
            // {
            //     testreferences.Add(new Reference(reference, DateTimeOffset.Now));
            // }
            //
            // List<Reference> references = new List<Reference>();
            // int i = 0;
            // string pubType;
            // foreach (RawReference rawReference in rawReferences)
            // {
            //     switch (i % 4)
            //     {
            //         case 0:
            //             pubType = "bog";
            //             break;
            //         case 1:
            //             pubType = "Online";
            //             break;
            //         case 2:
            //             pubType = "Tidsskrift";
            //             break;
            //         case 3:
            //             pubType = "uncategorized";
            //             break;
            //         default:
            //             pubType = "din far";
            //             break;
            //     }
            //     references.Add(new Reference(rawReference, pubType: pubType));
            //     i++;
            // }
            //
            // filteredReferences = references;


            IsApiKeyButtonEnabled = canProceed();
            IsExportButtonEnabled = false;
            IsStartButtonEnabled = false;
            IsImportButtonEnabled = false;

            IEnumerable<IGrouping<string, Reference>> referencesGroupedByPubType = filteredReferences.GroupBy(
                reference => reference.PubType.ToLower());
            foreach (IGrouping<string, Reference> grouping in referencesGroupedByPubType)
            {
                Debug.WriteLine($"{grouping.Key} has {grouping.Count()} reference(s)");
                sheet.SetActiveSheet(grouping.Key);
                sheet.AddReference(grouping);
            }

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
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Choose export folder",
                InitialFileName = "Behandlede_Referencer.xlsx",
                DefaultExtension = ".xlsx",

            };
            string? filePathToExportedFile = await saveFileDialog.ShowAsync(window);
            if (filePathToExportedFile is null)
            {
                IMsBoxWindow<ButtonResult> messageBoxStandardView = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Error", "Error in reading References from spreadsheet");
                messageBoxStandardView.Show();
            }
            else
            {
                Spreadsheet exportSheet = new Spreadsheet(filePathToExportedFile);
                exportSheet.Create();
                Export(exportSheet, filePathToExportedFile);
            }
        }


        /// <summary>
        /// Method called when this class receives a message on the default channel of type <c>FilePathsMessage.</c>
        /// </summary>
        /// <param name="message">The message received.</param>
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

        /// <summary>
        /// Reads all the references from the chosen Excel files.
        /// </summary>
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

        private void RunBackgroundSearchProcess(object sender, DoWorkEventArgs e)
        {
            _isRunning = true;
            int[] countRef = { 0, 0 };
            // Read all the references from the excel file
            ReadAllReferences();

            TotalReferences = rawReferences.Count;

            List<Reference> OverAllReferences = new List<Reference>();
            List<RawReference> leftOver = new List<RawReference>();

            // Filter the references
            FilterCollection instance = IFilterCollection.GetInstance();


            // Identify the references in the database
            // TODO: Implement the identification of the references

            foreach (RawReference reference in rawReferences.ToList())
            {
                Reference dbReference = DatabaseHelper.GetReference(reference.ExtractData().Title);

                if (dbReference.Title != "")
                {
                    OverAllReferences.Add(dbReference);
                }
                else
                {
                    leftOver.Add(reference);
                }
            }



            // If the database does not contain the reference, search for it in the internet
            ApiSearching apiSearching = new ApiSearching();
            // Call the apisearching method
            (List<Reference> listReferences, leftOver) = apiSearching.SearchReferences(leftOver);

            Console.WriteLine("References: " + rawReferences.Count);

            foreach (Reference reference in listReferences)
            {
                OverAllReferences.Add(reference);
            }

            // Categorize all the references
            foreach (Reference reference in OverAllReferences)
            {
                UpdateCounter(instance, countRef, reference);

                StartWorker.ReportProgress(0, countRef);
            }

            // Categorize all the remaining raw references
            foreach (RawReference rawreference in leftOver)
            {
                Reference reference = rawreference.ExtractData();
                reference.PubType = instance.categorize(reference);

                UpdateCounter(instance, countRef, reference);

                OverAllReferences.Add(reference);
                StartWorker.ReportProgress(0, countRef);
            }

            FilteredReferences = OverAllReferences;
        }

        private static void UpdateCounter(FilterCollection instance, int[] countRef, Reference reference)
        {
            // Call the categorize function.
            if (reference.PubType == null)
            {
                reference.PubType = instance.categorize(reference);
            }



            if (reference.PubType != "Uncategorized")
            {
                countRef[0]++;
            }

            else
            {
                countRef[1]++;
            }
        }

        private void CompletedBackgroundSearchProcess(object sender, RunWorkerCompletedEventArgs e)
        {
            _isRunning = false;
            IsExportButtonEnabled = true;
            IMsBoxWindow<ButtonResult> messageSaveFilterBox = (IMsBoxWindow<ButtonResult>)MessageBox.Avalonia
.MessageBoxManager.GetMessageBoxStandardWindow("Identifikations process", "Identifikations processen er blevet gennemført.");
            messageSaveFilterBox.Show();
        }

        private void ChangedBackgroundSearchProcess(object sender, ProgressChangedEventArgs e)
        {
            int[] countRef = e.UserState as int[];

            IdentifiedNumberCounter = countRef![0];
            UnIdentifiedNumberCounter = countRef[1];
        }
    }
}
