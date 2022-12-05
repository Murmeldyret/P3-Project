using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zenref.Ava.Models;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public partial class ExportViewModel : ObservableRecipient, IRecipient<SearchTermMessage>
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

        /// <summary>
        /// Property that hold the information from creating a new publicatino type
        /// </summary>
        private ObservableCollection<PublicationType> searchCriteria = new ObservableCollection<PublicationType>();
        private ObservableCollection<SearchPublicationType> searchTest = new ObservableCollection<SearchPublicationType>();
        private ObservableCollection<SearchPublicationType> searchTest2 = new ObservableCollection<SearchPublicationType>();

        /// <summary>
        /// A collection of the created publication types
        /// </summary>
        [ObservableProperty] 
        [NotifyCanExecuteChangedFor(nameof(EditPublicationTypeCommand))]
        private ObservableCollection<PublicationType> publicationTypes = new ObservableCollection<PublicationType>();
        
        /// <summary>
        /// Constructor, sets up the predefined publication types.
        /// And registers a message from the SearchCriteriaViewModel
        /// </summary>
        public ExportViewModel() 
            : base(WeakReferenceMessenger.Default)
        {
            searchTest.Add(new SearchPublicationType(searchTerm: "hello", searchSelectOperand: "OG", searchSelectField: "Titel"));
            searchTest2.Add(new SearchPublicationType(searchTerm: "hello2vuu", searchSelectOperand: "OG", searchSelectField: "Titel"));
            PublicationTypes.Add(new PublicationType("Bog", searchTest));
            PublicationTypes.Add(new PublicationType("Artikel", searchTest2));
            
            Messenger.Register<SearchTermMessage>(this, (r, m) =>
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
            PublicationTypes.Add(searchCriteria[0]);
        }

        [RelayCommand]
        private void OpenSearchCriteria(Window window)
        {
            SearchCriteriaView SearchView = new SearchCriteriaView();
            SearchView.Show();
        }

        /// <summary>
        /// Deletes the publication type with the same name as the one related to the delete button
        /// </summary>
        /// <param name="msg"></param>
        [RelayCommand]
        private void DeletePublicationType(string msg)
        {
            string text = (string)msg;
            
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (text == PublicationTypes[i].Name)
                {
                    PublicationTypes.RemoveAt(i);
                }
            }

        }
        
        /// <summary>
        /// Edits a specific publication type.
        /// It opens a new window with the information related to the publication type
        /// </summary>
        /// <param name="msg"></param>
        [RelayCommand]
        private void EditPublicationType(string msg)
        {
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (PublicationTypes[i].Name == msg)
                {
                    SearchCriteriaView SearchView = new SearchCriteriaView();
                    SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i].searchPublicationTypes, msg);
                    SearchView.Show();
                }
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

            

        }

        /// <summary>
        /// Exports to excel
        /// </summary>
        [RelayCommand]
        private void Export()
        {
            Console.WriteLine("Export");
        }
        
    }
}
