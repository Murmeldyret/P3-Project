using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenref.Ava.Models;
using Zenref.Ava.Views;

namespace Zenref.Ava.ViewModels
{
    public class PublicationType
    {
        public string Name { get; set; }
        public ObservableCollection<SearchPublicationType> searchPublicationTypes { get; set; }

        public PublicationType(string name, ObservableCollection<SearchPublicationType> searchPublicationTypes)
        {
            Name = name;
            this.searchPublicationTypes = searchPublicationTypes;
        }
    }
    public partial class ExportViewModel : ObservableRecipient, IRecipient<SearchTermMessage>
    {
        private ObservableCollection<PublicationType> searchCriteria = new ObservableCollection<PublicationType>();
        private ObservableCollection<SearchPublicationType> searchTest = new ObservableCollection<SearchPublicationType>();
        //private List<SearchPublicationType> searchCriteria = new List<SearchPublicationType>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePublicationTypeCommand))]
        [NotifyCanExecuteChangedFor(nameof(EditPublicationTypeCommand))]
        private ObservableCollection<PublicationType> publicationTypes = new ObservableCollection<PublicationType>();

        public ExportViewModel() 
            : base(WeakReferenceMessenger.Default)
        {
            searchTest.Add(new SearchPublicationType(searchTerm: "hello", searchSelectOperand: "OG", searchSelectField: "Titel"));
            PublicationTypes.Add(new PublicationType("Bog", searchTest));
            PublicationTypes.Add(new PublicationType("Artikel", searchTest));
            Messenger.Register<SearchTermMessage>(this, (r, m) =>
            {
                Receive(m);
            });
        }

        public void Receive(SearchTermMessage message)
        {
            searchCriteria = message.SearchPubCollection;
            foreach(PublicationType s in searchCriteria)
            {
                
                PublicationTypes.Add(new PublicationType(s.Name, s.searchPublicationTypes));
            }
        }

        [RelayCommand]
        private void OpenSearchCriteria(Window window)
        {
            SearchCriteriaWindow SearchView = new SearchCriteriaWindow();
            SearchView.ShowDialog(window);
        }

        [RelayCommand]
        private void DeletePublicationType(string text)
        {
            Debug.WriteLine(PublicationTypes.Count);
            //PublicationTypes.RemoveAt(PublicationTypes.Count - 1);
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (text == PublicationTypes[i].Name)
                {
                    Debug.WriteLine($"text: {text} == {publicationTypes[i].Name}");
                    PublicationTypes.RemoveAt(i);
                }
            }
        }


        [RelayCommand]
        private void EditPublicationType(ObservableCollection<PublicationType> window)
        {

            for(int i = 0; i < window.Count; i++)
            {
                Debug.WriteLine(window[i].Name);
            }
            /*
            SearchCriteriaWindow SearchView = new SearchCriteriaWindow();
            SearchView.DataContext = new SearchCriteriaViewModel(searchTest);
            SearchView.ShowDialog(window);
            */
            
            //window.ShowDialog(SearchView);
        }
    }
}
