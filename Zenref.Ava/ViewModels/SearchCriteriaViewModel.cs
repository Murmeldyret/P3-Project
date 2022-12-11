using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Channels;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Zenref.Ava.Models;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public class SearchTerms
    {
        private string searchString;
        public string SearchString
        {
            get
            {
                return this.searchString;
            }
            set
            {
                this.searchString = value;
            }
        }

        public SearchTerms(string searchString)
        {
            this.searchString = searchString;
        }
    }
    public partial class SearchCriteriaViewModel : ObservableObject
    {
        /// <summary>
        /// An observable collection of SearchPublicationType. It keeps track of the different search terms.
        /// 
        /// The ObservableProperty generates a public version of the property and implements INotifyPropertyChanged and
        /// INotifyPropertyChanging which automatically updates the View as needed.
        /// 
        /// The NotifyCanExecuteChangedFor property generates code that help executing commands form actions like a button click
        /// by binding to a method of the same name, but without the "Command" prefix.
        /// </summary>
        /*
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddSearchCriteriaCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteSearchCriteriaCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteAllSearchCriteriasCommand))]
        private ObservableCollection<SearchPublicationType> searchOption = new ObservableCollection<SearchPublicationType>();
        */
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddSearchCriteriaCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteSearchCriteriaCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteAllSearchCriteriasCommand))]
        private ObservableCollection<SearchTerms> searchOption = new ObservableCollection<SearchTerms>();
        
        private List<SearchTerms> originalSearchOptions = new List<SearchTerms>();

        /*
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        private ObservableCollection<PublicationType> pubTypes = new ObservableCollection<PublicationType>();
        */
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        private ObservableCollection<Filter> pubTypes = new ObservableCollection<Filter>();

        private ObservableCollection<Filter> oriPubTypes = new ObservableCollection<Filter>();

        /// Strings that keeps track of the different selected search options from the view
        [ObservableProperty] private bool isEditEnabled;
        [ObservableProperty] private bool isAddPubEnabled;
        
        
        private string? searchTextString;
        private string? SearchOperand;
        private string? SearchField;

        [ObservableProperty] 
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        private string? pubName;

        public SearchCriteriaViewModel()
        {
            SearchOption.Clear();
        }

        public SearchCriteriaViewModel(bool isAddPubEnabled)
        {
            this.isAddPubEnabled = isAddPubEnabled;
            isEditEnabled = false;
            SearchOption.Clear();
        }
        
        private bool isCanceled;

        public SearchCriteriaViewModel(Filter filter, bool isEditEnabled)
        {
            Console.WriteLine("Constructer struck hahahhhahhahahhaha");
            PubTypes.Clear();
            oriPubTypes.Clear();

            this.IsEditEnabled = isEditEnabled;
            this.isAddPubEnabled = false;
            PubName = filter.categoryName;
            searchOption = filter.filtQ;
            PubTypes.Add(new Filter(filter.filtQ, filter.filterQuery, filter.categoryName));
        }
        
        public SearchCriteriaViewModel(ObservableCollection<SearchTerms> searchOption, string pubName, bool isEditEnabled)
        {
            foreach (SearchTerms s in searchOption)
            {
                Console.WriteLine($"passed from export: {s.SearchString}");
            }
            SearchOption.Clear();
            SearchOption = searchOption;
            originalSearchOptions = searchOption.ToList();
            PubName = pubName;
            this.isEditEnabled = isEditEnabled;
        }

        /// <summary>
        /// The RelayCommand makes it into a new command, and is in relation to the NotifyCanExecuteChangedFor
        /// with the same name but "Command" as prefix
        /// 
        /// Sends the observable collection of search terms to the ExpandViewModel
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        private void AddPublicationType(Window window)
        {
            List<string> Search = new List<string>();
            foreach (SearchTerms s in SearchOption)      
            {
                Search.Add(s.SearchString);
            }
            
            
            PubTypes.Add(new Filter(SearchOption, Search, PubName));
            WeakReferenceMessenger.Default.Send<SearchTermMessage>(new SearchTermMessage(PubTypes));
            window.Close();
        }

        /// <summary>
        /// Edits the publication type 
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        private void Edit(Window window)
        { 
            isEditEnabled = false;
            window.Close();
        }

        /// <summary>
        /// Adds another search option with operand, searchField and text box
        /// </summary>
        [RelayCommand]
        private void AddSearchCriteria()
        {
            SearchOption.Add(new SearchTerms(searchString: searchTextString));
            
            /*
            foreach (SearchTerms s in SearchOption)
            {
                Console.WriteLine(s.SearchString);
            }
            */

            /*
            SearchOption.Add(new SearchPublicationType(
                searchTerm: SearchTextString, 
                searchSelectOperand: SearchOperand, 
                searchSelectField: SearchField));
            */
        }

        /// <summary>
        /// Deletes one search criteria
        /// </summary>
        [RelayCommand]
        private void DeleteSearchCriteria()
        {
            SearchOption.RemoveAt(SearchOption.Count - 1);

        }

        /// <summary>
        /// Deletes all search criterias
        /// </summary>
        [RelayCommand]
        private void DeleteAllSearchCriterias()
        {
            SearchOption.Clear();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        private void Cancel(Window window)
        {
            // Clear new observablelist and exchange with backup
            /*
            PubTypes.Clear();
            ObservableCollection<SearchTerms> osc = new ObservableCollection<SearchTerms>(originalSearchOptions);
            PubTypes.Add(new Filter(osc, PubName));
            */
            if (!PubTypes.Any())
            {
                window.Close();
            }
            else
            {
                isCanceled = true;
                Console.WriteLine(isCanceled);
                SearchOption.Clear();

                foreach (string s in pubTypes[0].filterQuery)
                {
                    searchOption.Add(new SearchTerms(s));
                }

                PubTypes[0].cancel = isCanceled;
                window.Close();
                
                
            }
        }

    }
}
