using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteAllCommand))]
        private ObservableCollection<SearchPublicationType> searchOption = new ObservableCollection<SearchPublicationType>();

        /// <summary>
        /// Public strings that keeps track of the different selected search options from the view
        /// </summary>
        public string? SearchTextString;
        public string? SearchOperand;
        public string? SearchField;
        

        public SearchCriteriaViewModel()
        {
            SearchOption.Clear();
        }


        /// <summary>
        /// The RelayCommand makes it into a new command, and is in relation to the NotifyCanExecuteChangedFor
        /// with the same name but "Command" as prefix
        /// 
        /// Combines all the search terms into one search
        /// </summary>
        [RelayCommand]
        private void Search()
        {
        }

        /// <summary>
        /// Adds another search option with operand, searchField and text box
        /// </summary>
        [RelayCommand]
        private void Add()
        {
            SearchOption.Add(new SearchPublicationType(
                searchTerm: SearchTextString, 
                searchSelectOperand: SearchOperand, 
                searchSelectField: SearchField));
            Debug.WriteLine(SearchOption[0].Operand[0]);
        }

        /// <summary>
        /// Deletes one search option
        /// </summary>
        [RelayCommand]
        private void Delete()
        {
            SearchOption.RemoveAt(SearchOption.Count - 1);

        }

        /// <summary>
        /// Deletes all search options
        /// </summary>
        [RelayCommand]
        private void DeleteAll()
        {
            SearchOption.Clear();
        }
    }
}
