using System.Collections.ObjectModel;
using Zenref.Ava.Models;

namespace Zenref.Ava.ViewModels;

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