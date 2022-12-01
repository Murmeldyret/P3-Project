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
using Zenref.Ava.ViewModels.Commands;
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
        private ObservableCollection<SearchPublicationType> searchTest2 = new ObservableCollection<SearchPublicationType>();
        //private List<SearchPublicationType> searchCriteria = new List<SearchPublicationType>();

        [ObservableProperty] 
        [NotifyCanExecuteChangedFor(nameof(EditPublicationTypeCommand))]
        private ObservableCollection<PublicationType> publicationTypes = new ObservableCollection<PublicationType>();
        
        public ICommand EditPublicationTypeCommmand { get; set; }

        public ExportViewModel() 
            : base(WeakReferenceMessenger.Default)
        {
            searchTest.Add(new SearchPublicationType(searchTerm: "hello", searchSelectOperand: "OG", searchSelectField: "Titel"));
            searchTest2.Add(new SearchPublicationType(searchTerm: "hello2vuu", searchSelectOperand: "OG", searchSelectField: "Titel"));
            PublicationTypes.Add(new PublicationType("Bog", searchTest));
            PublicationTypes.Add(new PublicationType("Artikel", searchTest2));

            foreach (PublicationType t in PublicationTypes)
            {
                Console.WriteLine(t.Name);
            }
            
            Messenger.Register<SearchTermMessage>(this, (r, m) =>
            {
                Receive(m);
            });
        }

        public void Receive(SearchTermMessage message)
        {
            searchCriteria = message.SearchPubCollection;
            Console.WriteLine(searchCriteria.Count);
            
            foreach(PublicationType s in PublicationTypes)
            {
                if (s.Name == searchCriteria[0].Name)
                {
                    s.searchPublicationTypes = searchCriteria[0].searchPublicationTypes;
                }

            }
        }

        [RelayCommand]
        private void OpenSearchCriteria(Window window)
        {
            SearchCriteriaWindow SearchView = new SearchCriteriaWindow();
            SearchView.Show();
        }

        [RelayCommand]
        private void DeletePublicationType(object msg)
        {
            Console.WriteLine(PublicationTypes.Count);
            //PublicationTypes.RemoveAt(PublicationTypes.Count - 1);
            string text = (string)msg;
            
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (text == PublicationTypes[i].Name)
                {
                    Console.WriteLine($"text: {text} == {publicationTypes[i].Name}");
                    PublicationTypes.RemoveAt(i);
                }
            }
            
        }

        
        [RelayCommand]
        private void EditPublicationType(string msg)
        {
            for (int i = 0; i < PublicationTypes.Count; i++)
            {
                if (PublicationTypes[i].Name == msg)
                {
                    SearchCriteriaWindow SearchView = new SearchCriteriaWindow();
                    SearchView.DataContext = new SearchCriteriaViewModel(PublicationTypes[i].searchPublicationTypes, msg);
                    SearchView.Show();
                }
            }
        }
    }
}
