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

namespace Zenref.Ava.ViewModels
{
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
        private ObservableCollection<SearchPublicationType> searchOption = new ObservableCollection<SearchPublicationType>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditCommand))]
        private ObservableCollection<PublicationType> pubTypes = new ObservableCollection<PublicationType>();

        /// Strings that keeps track of the different selected search options from the view
        private string? SearchTextString;
        private string? SearchOperand;
        private string? SearchField;

        [ObservableProperty] 
        [NotifyCanExecuteChangedFor(nameof(AddPublicationTypeCommand))]
        private string? pubName;

        public SearchCriteriaViewModel()
        {
            SearchOption.Clear();
        }

        public SearchCriteriaViewModel(ObservableCollection<SearchPublicationType> searchOption, string pubName)
        {
            SearchOption.Clear();
            SearchOption = searchOption;
            PubName = pubName;
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
            
            pubTypes.Add(new PublicationType(PubName, SearchOption));
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
            pubTypes.Add(new PublicationType(PubName, SearchOption));
            window.Close();
        }

        /// <summary>
        /// Adds another search option with operand, searchField and text box
        /// </summary>
        [RelayCommand]
        private void AddSearchCriteria()
        {

            SearchOption.Add(new SearchPublicationType(
                searchTerm: SearchTextString, 
                searchSelectOperand: SearchOperand, 
                searchSelectField: SearchField));

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
            window.Close();
        }

    }
}
