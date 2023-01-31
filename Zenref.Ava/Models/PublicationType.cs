using System.Collections.ObjectModel;
using Zenref.Ava.Models;

namespace Zenref.Ava.ViewModels;

// The PublicationType class represents a type of publication
public class PublicationType
{
    // Name of the publication type
    public string Name { get; set; }
    
    // Collection of SearchPublicationType objects associated with this PublicationType
    public ObservableCollection<SearchPublicationType> searchPublicationTypes { get; set; }
    
    // Constructor that initializes the name and searchPublicationTypes properties
    public PublicationType(string name, ObservableCollection<SearchPublicationType> searchPublicationTypes)
    {
        Name = name;
        this.searchPublicationTypes = searchPublicationTypes;
    }
}