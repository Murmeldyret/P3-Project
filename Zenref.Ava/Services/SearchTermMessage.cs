using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zenref.Ava.Models;

namespace Zenref.Ava.ViewModels;

public class SearchTermMessage
{
    public ObservableCollection<Filter> SearchPubCollection { get; init; }

    public SearchTermMessage(ObservableCollection<Filter> searchOption)
    {
        SearchPubCollection = searchOption;
    }
}