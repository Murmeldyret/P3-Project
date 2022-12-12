using System.Collections.ObjectModel;

namespace Zenref.Ava.ViewModels;

public class SearchTermMessage
{
    public ObservableCollection<PublicationType> SearchPubCollection { get; init; }

    public SearchTermMessage(ObservableCollection<PublicationType> searchOption)
    {
        SearchPubCollection = searchOption;
    }
}