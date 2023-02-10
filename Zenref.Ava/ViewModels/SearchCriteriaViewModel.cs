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
    // : The SearchTerms class contains a single string property, "SearchString", which represents a string to be used in a search.
    public class SearchTerms
    {
        private string searchString;
        
        // : Property to store the search string
        public string SearchString
        {
            // : Getter method to get the search string
            get
            {
                return this.searchString;
            }
            // : Setter method to set the search string
            set
            {
                this.searchString = value;
            }
        }

        // : Constructor to initialize the search string
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
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddSearchCriteriaCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteSearchCriteriaCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteAllSearchCriteriasCommand))]
        private ObservableCollection<SearchTerms> searchOption = new ObservableCollection<SearchTerms>();
        
        /// <summary>
        /// : A collection of `Filter` to keep track of different publication types.
        /// 
        /// : Implements `ObservableProperty` to generate a public version of the property and 
        /// : `INotifyPropertyChanged` to update the View as needed.
        /// 
        /// : Also implements `NotifyCanExecuteChangedFor` to generate code to execute commands 
        /// : like AddPublicationTypeCommand and EditCommand.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        private ObservableCollection<Filter> pubTypes = new ObservableCollection<Filter>();

        /// Strings that keeps track of the different selected search options from the view
        
        /// <summary>
        /// : A boolean property to determine if the edit option is enabled.
        /// : Implements `ObservableProperty` to generate a public version of the property and 
        /// : `INotifyPropertyChanged` to update the View as needed.
        /// </summary>
        [ObservableProperty] private bool isEditEnabled;
        
        /// <summary>
        /// : A boolean property to determine if the add publication option is enabled.
        /// : Implements `ObservableProperty` to generate a public version of the property and 
        /// : `INotifyPropertyChanged` to update the View as needed.
        /// </summary>
        [ObservableProperty] private bool isAddPubEnabled;
        
        /// <summary>
        /// : A string property to keep track of the search text string.
        /// : Implements `ObservableProperty` to generate a public version of the property and 
        /// : `INotifyPropertyChanged` to update the View as needed.
        /// </summary>
        private string? searchTextString;

        /// <summary>
        /// Observable property for the publication type name
        /// </summary>
        [ObservableProperty] 
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        private string? pubName;

        /// <summary>
        /// Constructor for the SearchCriteriaViewModel class that sets the edit mode to false and clears the search terms collection.
        /// </summary>
        public SearchCriteriaViewModel()
        {
            SearchOption.Clear();
        }

        /// <summary>
        /// Constructor for the SearchCriteriaViewModel class that sets the isAddPubEnabled flag and sets the edit mode to false.
        /// </summary>
        /// <param name="isAddPubEnabled">Boolean flag that indicates whether the add publication feature is enabled or not.</param>
        public SearchCriteriaViewModel(bool isAddPubEnabled)
        {
            this.isAddPubEnabled = isAddPubEnabled;
            isEditEnabled = false;
            SearchOption.Clear();
        }
        
        /// <summary>
        /// Constructor for the SearchCriteriaViewModel class that sets the values of the filter, edit mode and isAddPubEnabled flag.
        /// </summary>
        /// <param name="filter">Filter instance with values to be set.</param>
        /// <param name="isEditEnabled">Boolean flag that indicates whether the edit mode is enabled or not.</param>
        public SearchCriteriaViewModel(Filter filter, bool isEditEnabled)
        {
            // : Clears the collection of pubTypes
            PubTypes.Clear();
                
            // : Sets the IsEditEnabled property to the value passed as a parameter
            this.IsEditEnabled = isEditEnabled;
            
            // : Sets the isAddPubEnabled property to false
            this.isAddPubEnabled = false;
            
            // : Sets the value of the PubName property to the categoryName property of the filter instance
            PubName = filter.categoryName;
            
            //: Assigns the filtQ property of the filter instance to the searchOption property of this class instance
            searchOption = filter.filtQ;
            
            //: Adds a new filter instance to the pubTypes collection with filter details passed as parameters
            PubTypes.Add(new Filter(filter.filtQ, filter.filterQuery, filter.categoryName));
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
            // : Create a list of search strings from the SearchOption collection
            List<string> Search = new List<string>();
            foreach (SearchTerms s in SearchOption)      
            {
                Search.Add(s.SearchString);
            }
                
            // : Add a new Filter object to the PubTypes collection using the information from the SearchOption, Search and PubName variables
            PubTypes.Add(new Filter(SearchOption, Search, PubName));
            
            // : Use the WeakReferenceMessenger to send a SearchTermMessage with the updated PubTypes collection
            WeakReferenceMessenger.Default.Send<SearchTermMessage>(new SearchTermMessage(PubTypes));
            
            // : Close the current window
            window.Close();
        }

        /// <summary>
        /// Edits the publication type, and save the changes 
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        private void Edit(Window window)
        {
            // : Clear the filterQuery of the first item in PubTypes collection
            PubTypes[0].filterQuery.Clear();
            
            // : For each search term in filtQ of the first item in PubTypes collection
            foreach (SearchTerms s in PubTypes[0].filtQ)
            {
                // : Add the search string of the search term to the filterQuery of the first item in PubTypes collection
                PubTypes[0].filterQuery.Add(s.SearchString);
            }
            
            // : Set isEditEnabled to false
            isEditEnabled = false;
            
            // : Close the window object
            window.Close();
        }

        /// <summary>
        /// Adds another search option with operand, searchField and text box
        /// </summary>
        [RelayCommand]
        private void AddSearchCriteria()
        {
            SearchOption.Add(new SearchTerms(searchString: searchTextString));
        }

        /// <summary>
        /// Deletes one search criteria
        /// </summary>
        [RelayCommand]
        private void DeleteSearchCriteria()
        {
            if (SearchOption.Any())
            {
                // Altid den sidste som bliver slettet.
                SearchOption.RemoveAt(SearchOption.Count - 1);
            }

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
        /// Closes the window and discard unsaved changes
        /// </summary>
        /// <param name="window"></param>
        [RelayCommand]
        private void Cancel(Window window)
        {
            if (!PubTypes.Any())
            {
                window.Close();
            }
            else
            {
                SearchOption.Clear();

                foreach (string s in pubTypes[0].filterQuery)
                {
                    searchOption.Add(new SearchTerms(s));
                }

                window.Close();
                
                
            }
        }

    }
}
